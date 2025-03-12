using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Application.Encryption;
using Microsoft.Extensions.Configuration;

namespace ProjectManager.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly BlowfishEncryptionHelper _encryptionHelper;

        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _encryptionHelper = new BlowfishEncryptionHelper(configuration);
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
            var user = _mapper.Map<User>(userDto);
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

        // Регистрация пользователя с шифрованием пароля
        public async Task<bool> RegisterUserAsync(RegisterDto registerDto)
        {
            // Проверяем, существует ли пользователь с таким именем или email
            if (await _userRepository.ExistsAsync(registerDto.UserName, registerDto.Email))
            {
                return false;  // Пользователь с таким именем или email уже существует
            }

            // Генерация соли и шифрование пароля с помощью Blowfish
            string salt = _encryptionHelper.GenerateSalt();
            byte[] key = Convert.FromBase64String("simple_secret_key_1234"); // Используем секретный ключ из конфигурации или другого места
            var encryptedPassword = _encryptionHelper.Crypt(key, salt);

            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = encryptedPassword,  // Шифрованный пароль
                Role = "User"
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
