using Moq;
using Xunit;
using ProjectManager.Application.Services;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using ProjectManager.Application.Interfaces.ValidationInterfaces;

namespace Application.Tests.ServicesTests.UserServiceHandlerTests
{
    public class GetByIdAsyncHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _service;

        public GetByIdAsyncHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new UserService(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                Mock.Of<IConfiguration>(),
                Mock.Of<IAddUserValidationService>()
            );
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUserDto_WhenUserExists()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                UserId = userId,
                UserName = "testuser",
                Email = "testuser@example.com"
            };

            var userDto = new UserDto
            {
                UserId = userId,
                UserName = "testuser",
                Email = "testuser@example.com"
            };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);
            
            var result = await _service.GetByIdAsync(userId);
            
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("testuser", result.UserName);
            Assert.Equal("testuser@example.com", result.Email);
            _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<UserDto>(user), Times.Once);
        }

    }
}
