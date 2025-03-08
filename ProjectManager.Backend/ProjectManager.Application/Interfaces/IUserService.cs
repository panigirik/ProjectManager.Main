using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(string id);
    Task CreateAsync(UserDto userDto);
    Task UpdateAsync(UserDto userDto);
    Task DeleteAsync(string id);
}