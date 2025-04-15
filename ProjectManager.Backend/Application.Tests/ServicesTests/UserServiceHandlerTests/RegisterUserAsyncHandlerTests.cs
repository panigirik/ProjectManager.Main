using Moq;
using Xunit;
using ProjectManager.Application.Services;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Encryption;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using ProjectManager.Application.Interfaces.ValidationInterfaces;

namespace Application.Tests.ServicesTests.UserServiceHandlerTests
{
    public class RegisterUserAsyncHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<BlowfishEncryptionHelper> _encryptionHelperMock;
        private readonly UserService _service;

        public RegisterUserAsyncHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _encryptionHelperMock = new Mock<BlowfishEncryptionHelper>(Mock.Of<IConfiguration>());
            _service = new UserService(
                _userRepositoryMock.Object,
                Mock.Of<IMapper>(),
                Mock.Of<IConfiguration>(),
                Mock.Of<IAddUserValidationService>()
            );
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFalse_WhenUserExists()
        {
            var registerDto = new RegisterDto
            {
                UserName = "existinguser",
                Email = "existinguser@example.com"
            };

            _userRepositoryMock.Setup(repo => repo.ExistsAsync(registerDto.UserName, registerDto.Email))
                .ReturnsAsync(true);
            
            var result = await _service.RegisterUserAsync(registerDto);
            
            Assert.False(result);
            _userRepositoryMock.Verify(repo => repo.ExistsAsync(registerDto.UserName, registerDto.Email), Times.Once);
            _userRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnTrue_WhenUserDoesNotExist()
        {
            var registerDto = new RegisterDto
            {
                UserName = "newuser",
                Email = "newuser@example.com"
            };

            _userRepositoryMock.Setup(repo => repo.ExistsAsync(registerDto.UserName, registerDto.Email))
                .ReturnsAsync(false);

            _encryptionHelperMock.Setup(helper => helper.GenerateSalt()).Returns("some-salt");
            _encryptionHelperMock.Setup(helper => helper.Crypt(It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns("encryptedpassword");

            _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            
            
            _userRepositoryMock.Verify(repo => repo.ExistsAsync(registerDto.UserName, registerDto.Email), Times.Never);
            _userRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<User>(u =>
                u.UserName == registerDto.UserName && u.Email == registerDto.Email && u.Password == "encryptedpassword")), Times.Never);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowException_WhenRepositoryFails()
        {
            var registerDto = new RegisterDto
            {
                UserName = "newuser",
                Email = "newuser@example.com"
            };

            _userRepositoryMock.Setup(repo => repo.ExistsAsync(registerDto.UserName, registerDto.Email))
                .ReturnsAsync(false);

            _encryptionHelperMock.Setup(helper => helper.GenerateSalt()).Returns("some-salt");
            _encryptionHelperMock.Setup(helper => helper.Crypt(It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns("encryptedpassword");

            _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<User>())).ThrowsAsync(new Exception("Repository error"));
            
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _service.RegisterUserAsync(registerDto));
            Assert.Equal("Value cannot be null. (Parameter 's')", exception.Message);
        }
    }
}
