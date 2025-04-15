using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Interfaces.ValidationInterfaces;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Interfaces.Jwt;
using Xunit;

namespace Application.Tests.ServicesTests.AuthentificationServiceHandlerTests;

public class LoginAsyncHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock = new();
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock = new();
    private readonly Mock<ILoginValidationService> _validationServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    private readonly AuthentificationService _service;

    public LoginAsyncHandlerTests()
    {
        _service = new AuthentificationService(
            _userRepositoryMock.Object,
            _refreshTokenRepositoryMock.Object,
            _jwtTokenServiceMock.Object,
            _validationServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnLoginResult_WhenCredentialsAreValid()
    {
        var request = new LoginRequest { Email = "test@example.com", Password = "password123" };
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = Roles.user
        };

        var refreshTokenDto = new RefreshToken
        {
            Token = "refresh-token",
            Expires = DateTime.UtcNow.AddDays(7)
        };

        _validationServiceMock
            .Setup(x => x.ValidateLoginRequestAsync(request))
            .ReturnsAsync(new ValidationResult());

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _jwtTokenServiceMock
            .Setup(x => x.GenerateAccessToken(user.UserId, user.Role))
            .Returns("access-token");

        _jwtTokenServiceMock
            .Setup(x => x.GenerateRefreshToken(user.UserId))
            .Returns(refreshTokenDto);

        _mapperMock
            .Setup(x => x.Map<RefreshToken>(refreshTokenDto))
            .Returns(new RefreshToken());

        _refreshTokenRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);
        
        var result = await _service.LoginAsync(request);
        
        Assert.Equal("access-token", result.AccessToken);
        Assert.Equal("refresh-token", result.RefreshToken);
    }


    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorizedException_WhenUserNotFound()
    {
        var request = new LoginRequest { Email = "notfound@example.com", Password = "password" };
        _validationServiceMock.Setup(x => x.ValidateLoginRequestAsync(request)).Returns(Task.CompletedTask as Task<ValidationResult>);
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(request.Email)).ReturnsAsync((User)null!);

        await Assert.ThrowsAsync<NullReferenceException>(() => _service.LoginAsync(request));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowUnauthorizedException_WhenPasswordInvalid()
    {
        var request = new LoginRequest { Email = "test@example.com", Password = "wrongpassword" };

        var validationResult = new ValidationResult(); 

        _validationServiceMock
            .Setup(x => x.ValidateLoginRequestAsync(request))
            .ReturnsAsync(validationResult);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _service.LoginAsync(request));
        Assert.Equal("Invalid email or password", exception.Message);
    }




    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenValidationFails()
    {
        var request = new LoginRequest { Email = "", Password = "" };
        _validationServiceMock.Setup(x => x.ValidateLoginRequestAsync(request))
            .ThrowsAsync(new ValidationException("Validation failed"));

        await Assert.ThrowsAsync<ValidationException>(() => _service.LoginAsync(request));
    }
}
