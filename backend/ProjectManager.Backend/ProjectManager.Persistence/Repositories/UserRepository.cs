using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

public class UserRepository: IUserRepository
{
    private readonly UserDbContext _userDbContext;

    public UserRepository(UserDbContext context)
    {
        _userDbContext = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userDbContext.Users.Find(_ => true).ToListAsync();
    }
        

    public async Task<User> GetByIdAsync(Guid id)
    {
       return await _userDbContext.Users.Find(u => u.UserId == id).FirstOrDefaultAsync();
    }


    public async Task CreateAsync(User user)
    { 
        await _userDbContext.Users.InsertOneAsync(user);
    }


    public async Task UpdateAsync(User user)
    {
        await _userDbContext.Users.ReplaceOneAsync(u => u.UserId == user.UserId, user);
    }
        
    
    public async Task<bool> ExistsAsync(string userName, string email)
    {
        return await _userDbContext.Users
            .Find(u => u.UserName == userName || u.Email == email)
            .AnyAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userDbContext.Users.DeleteOneAsync(u => u.UserId == id);
    }
    
    /// <summary>
    /// Получает пользователя по email.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>Пользователь с указанным email.</returns>
    public async Task<User> GetByEmailAsync(string email)
    {
        return await _userDbContext.Users.Find(u => u.Email == email).FirstOrDefaultAsync();
    }
        
}