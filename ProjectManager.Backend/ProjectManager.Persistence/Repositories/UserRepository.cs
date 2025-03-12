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

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _userDbContext.Users.Find(_ => true).ToListAsync();

    public async Task<User> GetByIdAsync(Guid id) =>
        await _userDbContext.Users.Find(u => u.UserId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User user) =>
        await _userDbContext.Users.InsertOneAsync(user);

    public async Task UpdateAsync(User user) =>
        await _userDbContext.Users.ReplaceOneAsync(u => u.UserId == user.UserId, user);

    public async Task DeleteAsync(Guid id) =>
        await _userDbContext.Users.DeleteOneAsync(u => u.UserId == id);
}