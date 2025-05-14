using Moq;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;
using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces.ValidationInterfaces;
using ProjectManager.Domain.Enums;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Interfaces.Jwt;
using Xunit;

namespace Application.Tests.ServicesTests.AuthentificationServiceHandlerTests;

public class RefreshTokenAsyncHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<ILoginValidationService> _validationServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuthentificationService _service;

    public RefreshTokenAsyncHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _validationServiceMock = new Mock<ILoginValidationService>();
        _mapperMock = new Mock<IMapper>();

        _service = new AuthentificationService(
            _userRepositoryMock.Object,
            _refreshTokenRepositoryMock.Object,
            _jwtTokenServiceMock.Object,
            _validationServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldReturnNewAccessToken_WhenRefreshTokenIsValid()
    {
        var userId = Guid.NewGuid();
        var refreshToken = "valid_refresh_token";
        var storedToken = new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            IsRevoked = false,
            Expires = DateTime.UtcNow.AddHours(1)
        };
        var user = new User { UserId = userId, Role = Roles.user };
        var userDto = new UserDto { UserId = userId,  Role = Roles.user };

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(storedToken);
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);
        _mapperMock
            .Setup(x => x.Map<UserDto>(user))
            .Returns(userDto);
        _jwtTokenServiceMock
            .Setup(x => x.GenerateAccessToken(userDto.UserId, userDto.Role))
            .Returns("new_access_token");
        
        var result = await _service.RefreshTokenAsync(userId, refreshToken);
        
        Assert.Equal("new_access_token", result);
        _refreshTokenRepositoryMock.Verify(x => x.GetByUserIdAsync(userId), Times.Once);
        _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        _jwtTokenServiceMock.Verify(x => x.GenerateAccessToken(userDto.UserId, userDto.Role), Times.Once);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowUnauthorizedException_WhenRefreshTokenIsInvalid()
    {
        var userId = Guid.NewGuid();
        var refreshToken = "invalid_refresh_token";
        var storedToken = new RefreshToken
        {
            UserId = userId,
            Token = "wrong_token",
            IsRevoked = false,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(storedToken);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _service.RefreshTokenAsync(userId, refreshToken));
        Assert.Equal("Invalid or expired refresh token", exception.Message);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowUnauthorizedException_WhenTokenIsRevoked()
    {
        var userId = Guid.NewGuid();
        var refreshToken = "valid_refresh_token";
        var storedToken = new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            IsRevoked = true,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(storedToken);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _service.RefreshTokenAsync(userId, refreshToken));
        Assert.Equal("Invalid or expired refresh token", exception.Message);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowUnauthorizedException_WhenTokenIsExpired()
    {
        var userId = Guid.NewGuid();
        var refreshToken = "valid_refresh_token";
        var storedToken = new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            IsRevoked = false,
            Expires = DateTime.UtcNow.AddHours(-1)
        };

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(storedToken);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _service.RefreshTokenAsync(userId, refreshToken));
        Assert.Equal("Invalid or expired refresh token", exception.Message);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldThrowUnauthorizedException_WhenUserNotFound()
    {
        var userId = Guid.NewGuid();
        var refreshToken = "valid_refresh_token";
        var storedToken = new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            IsRevoked = false,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(storedToken);
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync((User)null);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _service.RefreshTokenAsync(userId, refreshToken));
        Assert.Equal("User not found", exception.Message);
    }
}
