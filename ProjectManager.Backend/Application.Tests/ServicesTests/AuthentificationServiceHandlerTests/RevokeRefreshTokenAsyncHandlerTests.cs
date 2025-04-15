using AutoMapper;
using Moq;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Interfaces.ValidationInterfaces;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Interfaces.Jwt;
using Xunit;

namespace Application.Tests.ServicesTests.AuthentificationServiceHandlerTests;

public class RevokeRefreshTokenAsyncHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<ILoginValidationService> _validationServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuthentificationService _service;

    public RevokeRefreshTokenAsyncHandlerTests()
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
    public async Task RevokeRefreshTokenAsync_ShouldMarkTokenAsRevoked_WhenTokenExists()
    {
        var tokenId = Guid.NewGuid();
        var token = new RefreshToken { RefreshTokenId = tokenId, IsRevoked = false };

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByIdAsync(tokenId))
            .ReturnsAsync(token);

        _refreshTokenRepositoryMock
            .Setup(x => x.UpdateAsync(token))
            .Returns(Task.CompletedTask);
        
        await _service.RevokeRefreshTokenAsync(tokenId);
        
        Assert.True(token.IsRevoked);
        _refreshTokenRepositoryMock.Verify(x => x.UpdateAsync(It.Is<RefreshToken>(t => t.IsRevoked)), Times.Once);
    }

    [Fact]
    public async Task RevokeRefreshTokenAsync_ShouldThrowNotFoundException_WhenTokenDoesNotExist()
    {
        var tokenId = Guid.NewGuid();

        _refreshTokenRepositoryMock
            .Setup(x => x.GetByIdAsync(tokenId)).ReturnsAsync((RefreshToken)null);
        
        await Assert.ThrowsAsync<NotFoundException>(() => _service.RevokeRefreshTokenAsync(tokenId));
    }
}