using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interfaces;

public interface IColumnRepository
{
    Task<IEnumerable<Column>> GetAllAsync();
    Task<Column> GetByIdAsync(string id);
    Task CreateAsync(Column column);
    Task UpdateAsync(Column column);
    Task DeleteAsync(string id);
}