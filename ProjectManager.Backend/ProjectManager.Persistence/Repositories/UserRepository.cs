using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

public class UserRepository: IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(UserDbContext context)
    {
        _users = context.Users;
    }

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _users.Find(_ => true).ToListAsync();

    public async Task<User> GetByIdAsync(string id) =>
        await _users.Find(u => u.UserId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User user) =>
        await _users.InsertOneAsync(user);

    public async Task UpdateAsync(User user) =>
        await _users.ReplaceOneAsync(u => u.UserId == user.UserId, user);

    public async Task DeleteAsync(string id) =>
        await _users.DeleteOneAsync(u => u.UserId == id);
}