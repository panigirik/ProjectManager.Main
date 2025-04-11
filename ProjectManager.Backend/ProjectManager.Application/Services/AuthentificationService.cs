using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.Interfaces.ValidationInterfaces;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Interfaces.Jwt;

namespace ProjectManager.Application.Services;

public class AuthentificationService: IAuthentificationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILoginValidationService _validationService;
    private readonly IRefreshTokenRepository _repository;
    private readonly IMapper _mapper;

    public AuthentificationService(IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenService jwtTokenService,
        ILoginValidationService validationService,
        IRefreshTokenRepository repository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService;
        _validationService = validationService;
        _mapper = mapper;
        _repository = repository;
    }
    
    /// <summary>
    /// Выполняет аутентификацию пользователя.
    /// </summary>
    /// <param name="request">Объект <see cref="LoginRequest"/>, содержащий данные для входа.</param>
    /// <returns>Возвращает объект <see cref="LoginResult"/>, содержащий access и refresh токены.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Выбрасывается, если входные данные некорректны или учетные данные неверны.
    /// </exception>
    public async Task<LoginResult> LoginAsync(LoginRequest request)
    {
        await _validationService.ValidateLoginRequestAsync(request);

        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null || string.IsNullOrWhiteSpace(user.Password))
            throw new UnauthorizedException("Invalid email or password");

        if (!BCrypt.Net.BCrypt.Verify(request.Password ?? "", user.Password))
            throw new UnauthorizedException("Invalid email or password");


        var accessToken = _jwtTokenService.GenerateAccessToken(user.UserId, user.Role);
        var refreshTokenDto = _jwtTokenService.GenerateRefreshToken(user.UserId);

        var refreshTokenEntity = _mapper.Map<RefreshToken>(refreshTokenDto);
        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        return new LoginResult(accessToken, refreshTokenDto.Token);
    }

    public async Task LogoutAsync(Guid userId)
    {
        await RevokeRefreshTokenAsync(userId);
    }

    /// <summary>
    /// Отзывает refresh-токен по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор refresh-токена.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    public async Task RevokeRefreshTokenAsync(Guid id)
    {
        var token = await _repository.GetByIdAsync(id);
        if (token == null)
        {
            throw new NotFoundException("token not found");
        }
        token.IsRevoked = true;
        await _repository.UpdateAsync(token);
    
    }
    
    
    /// <summary>
    /// Выполняет обновление access-токена на основе refresh-токена.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="refreshToken">Текущий refresh-токен пользователя.</param>
    /// <returns>Новый access-токен.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Выбрасывается, если refresh-токен недействителен, истек или пользователь не найден.
    /// </exception>
    public async Task<string> RefreshTokenAsync(Guid userId, string refreshToken)
    {
        var storedToken = await _refreshTokenRepository.GetByUserIdAsync(userId);

        if (storedToken == null || 
            storedToken.Token != refreshToken || 
            storedToken.IsRevoked || 
            storedToken.Expires < DateTime.UtcNow)
        {
            throw new UnauthorizedException("Invalid or expired refresh token");
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new UnauthorizedException("User not found");

        var userDto = _mapper.Map<UserDto>(user);

        return _jwtTokenService.GenerateAccessToken(userDto.UserId, userDto.Role);
    }
    
}