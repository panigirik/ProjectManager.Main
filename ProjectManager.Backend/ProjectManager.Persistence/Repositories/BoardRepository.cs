using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

public class BoardRepository: IBoardRepository
{
    private readonly BoardDbContext _boardDbContext;

    public BoardRepository(BoardDbContext context)
    {
        _boardDbContext = context;
    }

    public async Task<IEnumerable<Board>> GetAllAsync() =>
        await _boardDbContext.Boards.Find(_ => true).ToListAsync();

    public async Task<Board> GetByIdAsync(string id) =>
        await _boardDbContext.Boards.Find(b => b.BoardId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Board board) =>
        await _boardDbContext.Boards.InsertOneAsync(board);

    public async Task UpdateAsync(Board board) =>
        await _boardDbContext.Boards.ReplaceOneAsync(b => b.BoardId == board.BoardId, board);

    public async Task DeleteAsync(string id) =>
        await _boardDbContext.Boards.DeleteOneAsync(b => b.BoardId == id);
}