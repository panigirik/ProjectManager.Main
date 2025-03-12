using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(Guid id);
    Task CreateAsync(UserDto userDto);
    Task UpdateAsync(UserDto userDto);
    Task DeleteAsync(Guid id);

    Task<bool> RegisterUserAsync(RegisterDto registerDto);

    Task<bool> UserExistsAsync(RegisterDto registerDto);
}