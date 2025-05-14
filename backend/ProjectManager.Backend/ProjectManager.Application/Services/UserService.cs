using System.Text;
using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Encryption;
using Microsoft.Extensions.Configuration;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Interfaces.ValidationInterfaces;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly BlowfishEncryptionHelper _encryptionHelper;
        private readonly string _encryptionKey;
        private readonly IAddUserValidationService _userValidationService;

        public UserService(IUserRepository userRepository,
            IMapper mapper,
            IConfiguration configuration,
            IAddUserValidationService userValidationService)
        {
            _userRepository = userRepository;
            _userValidationService = userValidationService;
            _mapper = mapper;
            _encryptionHelper = new BlowfishEncryptionHelper(configuration);
            _encryptionKey = configuration["EncryptionKey"];
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task CreateAsync(UserDto userDto)
        {
            await _userValidationService.ValidateAsync(userDto);
            if (userDto == null)
            {
                throw new BadRequestException("User data is null.");
            }
            var user = _mapper.Map<User>(userDto);
            user.UserId = Guid.NewGuid();
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password, workFactor: 12);
            await _userRepository.CreateAsync(user);
        }

        public async Task UpdateAsync(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> RegisterUserAsync(RegisterDto registerDto)
        {
            if (await _userRepository.ExistsAsync(registerDto.UserName, registerDto.Email))
            {
                return false; 
            }

            string salt = _encryptionHelper.GenerateSalt();
            string encryptedPassword = _encryptionHelper.Crypt(Encoding.UTF8.GetBytes(_encryptionKey), salt);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = encryptedPassword,
                Role = Roles.creator
            };

            await _userRepository.CreateAsync(user);
            return true;
        }

        public async Task<bool> UserExistsAsync(RegisterDto registerDto)
        {
            bool exists = await _userRepository.ExistsAsync(registerDto.UserName, registerDto.Email);
    
            return exists;
        }

    }
}
