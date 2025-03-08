using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(string id);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(string id);
}