using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(Guid id);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    
    Task<bool> ExistsAsync(string userName, string email);
    
    /// <summary>
    /// Получить пользователя по email.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>Пользователь.</returns>
    Task<User> GetByEmailAsync(string email);
}