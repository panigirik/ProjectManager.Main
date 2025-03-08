using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;

namespace ProjectManager.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper; // Внедряем IMapper

        // Конструктор с внедрением IMapper
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            // Получаем все users из репозитория
            var users = await _userRepository.GetAllAsync();
            
            // Преобразуем сущности в DTO
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            // Получаем user по id
            var user = await _userRepository.GetByIdAsync(id);
            
            // Преобразуем сущность в DTO
            return _mapper.Map<UserDto>(user);
        }

        public async Task CreateAsync(UserDto userDto)
        {
            // Преобразуем DTO в сущность
            var user = _mapper.Map<User>(userDto);
            
            // Создаем user через репозиторий
            await _userRepository.CreateAsync(user);
        }

        public async Task UpdateAsync(UserDto userDto)
        {
            // Преобразуем DTO в сущность
            var user = _mapper.Map<User>(userDto);
            
            // Обновляем user через репозиторий
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            // Удаляем user через репозиторий
            await _userRepository.DeleteAsync(id);
        }
    }
}