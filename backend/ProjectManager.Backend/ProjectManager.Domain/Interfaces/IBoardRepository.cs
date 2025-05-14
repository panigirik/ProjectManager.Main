using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interfaces;

public interface IBoardRepository
{
    Task<IEnumerable<Board>> GetAllAsync();
    Task<Board> GetByIdAsync(Guid id);
    Task CreateAsync(Board board);
    Task UpdateAsync(Board board);
    Task DeleteAsync(Guid id);
}