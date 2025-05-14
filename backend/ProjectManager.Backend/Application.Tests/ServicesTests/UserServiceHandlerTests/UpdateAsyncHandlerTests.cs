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
    public class UpdateAsyncHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _service;

        public UpdateAsyncHandlerTests()
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
        public async Task UpdateAsync_ShouldUpdateUser_WhenUserDtoIsValid()
        {
            var userDto = new UserDto
            {
                UserId = Guid.NewGuid(),
                UserName = "updateduser",
                Email = "updateduser@example.com",
                Password = "newpassword123"
            };

            var user = new User
            {
                UserId = userDto.UserId,
                UserName = "olduser",
                Email = "olduser@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("oldpassword123", workFactor: 12)
            };

            _mapperMock.Setup(mapper => mapper.Map<User>(userDto)).Returns(user);
            _userRepositoryMock.Setup(repo => repo.UpdateAsync(user)).Returns(Task.CompletedTask);
            
            await _service.UpdateAsync(userDto);
            
            _mapperMock.Verify(mapper => mapper.Map<User>(userDto), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUpdateFails()
        {
            var userDto = new UserDto
            {
                UserId = Guid.NewGuid(),
                UserName = "updateduser",
                Email = "updateduser@example.com",
                Password = "newpassword123"
            };

            var user = new User
            {
                UserId = userDto.UserId,
                UserName = "olduser",
                Email = "olduser@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("oldpassword123", workFactor: 12)
            };

            _mapperMock.Setup(mapper => mapper.Map<User>(userDto)).Returns(user);
            _userRepositoryMock.Setup(repo => repo.UpdateAsync(user)).ThrowsAsync(new Exception("Update failed"));
            
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(userDto));
            Assert.Equal("Update failed", exception.Message);
        }
    }
}
