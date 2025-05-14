using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

/// <summary>
/// Репозиторий для работы с refresh токенами в MongoDB.
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly RefreshTokenDbContext _refreshTokenDbContext;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokenRepository"/>.
    /// </summary>
    /// <param name="database">Экземпляр MongoDB базы данных.</param>
    public RefreshTokenRepository(RefreshTokenDbContext refreshTokenDbContext)
    {
        _refreshTokenDbContext = refreshTokenDbContext;
    }

    /// <summary>
    /// Получает refresh токен по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор refresh токена.</param>
    public async Task<RefreshToken> GetByIdAsync(Guid id)
    {
        var filter = Builders<RefreshToken>.Filter.Eq(rt => rt.RefreshTokenId, id);
        return await _refreshTokenDbContext.RefreshTokens.Find(filter).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Получает все refresh токены.
    /// </summary>
    public async Task<IEnumerable<RefreshToken>> GetAllAsync()
    {
        return await _refreshTokenDbContext.RefreshTokens.Find(_ => true).ToListAsync();
    }

    /// <summary>
    /// Добавляет новый refresh токен.
    /// </summary>
    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _refreshTokenDbContext.RefreshTokens.InsertOneAsync(refreshToken);
    }

    /// <summary>
    /// Обновляет существующий refresh токен.
    /// </summary>
    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        var filter = Builders<RefreshToken>.Filter.Eq(rt => rt.RefreshTokenId, refreshToken.RefreshTokenId);
        await _refreshTokenDbContext.RefreshTokens.ReplaceOneAsync(filter, refreshToken);
    }

    /// <summary>
    /// Удаляет refresh токен по объекту.
    /// </summary>
    public async Task DeleteAsync(RefreshToken token)
    {
        var filter = Builders<RefreshToken>.Filter.Eq(rt => rt.RefreshTokenId, token.RefreshTokenId);
        await _refreshTokenDbContext.RefreshTokens.DeleteOneAsync(filter);
    }

    /// <summary>
    /// Получает refresh токен по идентификатору пользователя.
    /// </summary>
    public async Task<RefreshToken> GetByUserIdAsync(Guid userId)
    {
        var filter = Builders<RefreshToken>.Filter.Eq(rt => rt.UserId, userId);
        return await _refreshTokenDbContext.RefreshTokens.Find(filter).FirstOrDefaultAsync();
    }
}
