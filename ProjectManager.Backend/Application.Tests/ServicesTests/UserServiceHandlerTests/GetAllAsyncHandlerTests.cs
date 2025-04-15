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
    public class GetAllAsyncHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _service;

        public GetAllAsyncHandlerTests()
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
        public async Task GetAllAsync_ShouldReturnUserDtos_WhenUsersExist()
        {
            var users = new List<User>
            {
                new User { UserId = Guid.NewGuid(), UserName = "testuser1", Email = "test1@example.com" },
                new User { UserId = Guid.NewGuid(), UserName = "testuser2", Email = "test2@example.com" }
            };

            var userDtos = new List<UserDto>
            {
                new UserDto { UserId = users[0].UserId, UserName = users[0].UserName, Email = users[0].Email },
                new UserDto { UserId = users[1].UserId, UserName = users[1].UserName, Email = users[1].Email }
            };

            _userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>())).Returns(userDtos);
            
            var result = await _service.GetAllAsync();
            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, u => u.UserName == "testuser1");
            Assert.Contains(result, u => u.UserName == "testuser2");
            _userRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<UserDto>>(users), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            _userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<User>());
            
            var result = await _service.GetAllAsync();
            
            Assert.NotNull(result);
            Assert.Empty(result);
            _userRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}
