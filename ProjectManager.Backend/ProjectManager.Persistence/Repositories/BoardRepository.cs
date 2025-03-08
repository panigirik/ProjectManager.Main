using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

public class BoardRepository: IBoardRepository
{
    private readonly IMongoCollection<Board> _boards;

    public BoardRepository(BoardDbContext context)
    {
        _boards = context.Boards;
    }

    public async Task<IEnumerable<Board>> GetAllAsync() =>
        await _boards.Find(_ => true).ToListAsync();

    public async Task<Board> GetByIdAsync(string id) =>
        await _boards.Find(b => b.BoardId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Board board) =>
        await _boards.InsertOneAsync(board);

    public async Task UpdateAsync(Board board) =>
        await _boards.ReplaceOneAsync(b => b.BoardId == board.BoardId, board);

    public async Task DeleteAsync(string id) =>
        await _boards.DeleteOneAsync(b => b.BoardId == id);
}