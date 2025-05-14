using Moq;
using Xunit;
using ProjectManager.Application.Services;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Interfaces.ValidationInterfaces;

namespace Application.Tests.ServicesTests.UserServiceHandlerTests
{
    public class CreateAsyncHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAddUserValidationService> _userValidationServiceMock;
        private readonly UserService _service;

        public CreateAsyncHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userValidationServiceMock = new Mock<IAddUserValidationService>();

            _service = new UserService(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                Mock.Of<IConfiguration>(),
                _userValidationServiceMock.Object
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateUser_WhenUserDtoIsValid()
        {
            var userDto = new UserDto
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                Password = "password123"
            };

            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = "testuser",
                Email = "testuser@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password123", workFactor: 12)
            };

            _userValidationServiceMock.Setup(service => service.ValidateAsync(userDto)).Returns(Task.CompletedTask);
            _mapperMock.Setup(mapper => mapper.Map<User>(userDto)).Returns(user);
            _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            
            await _service.CreateAsync(userDto);
            
            _userValidationServiceMock.Verify(service => service.ValidateAsync(userDto), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<User>(userDto), Times.Once);
            _userRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<User>(u => u.UserName == userDto.UserName && u.Email == userDto.Email)), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowBadRequestException_WhenUserDtoIsNull()
        {
            UserDto userDto = null;
            
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(userDto));
            Assert.Equal("Bad request", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenUserValidationFails()
        {
            var userDto = new UserDto
            {
                UserName = "testuser",
                Email = "testuser@example.com",
                Password = "password123"
            };

            _userValidationServiceMock.Setup(service => service.ValidateAsync(userDto)).ThrowsAsync(new Exception("Validation failed"));
            
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(userDto));
            Assert.Equal("Validation failed", exception.Message);
            _userValidationServiceMock.Verify(service => service.ValidateAsync(userDto), Times.Once);
        }
    }
}
