using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interfaces;

public interface IBoardRepository
{
    Task<IEnumerable<Board>> GetAllAsync();
    Task<Board> GetByIdAsync(string id);
    Task CreateAsync(Board board);
    Task UpdateAsync(Board board);
    Task DeleteAsync(string id);
}