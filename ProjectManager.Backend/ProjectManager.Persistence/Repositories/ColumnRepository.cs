using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

public class ColumnRepository: IColumnRepository
{
    private readonly ColumnDbContext _columnDbContext;

    public ColumnRepository(ColumnDbContext context)
    {
        _columnDbContext = context;
    }

    public async Task<IEnumerable<Column>> GetAllAsync() =>
        await _columnDbContext.Columns.Find(_ => true).ToListAsync();

    public async Task<Column> GetByIdAsync(string id) =>
        await _columnDbContext.Columns.Find(c => c.ColumnId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Column column) =>
        await _columnDbContext.Columns.InsertOneAsync(column);

    public async Task UpdateAsync(Column column) =>
        await _columnDbContext.Columns.ReplaceOneAsync(c => c.ColumnId == column.ColumnId, column);

    public async Task DeleteAsync(string id) =>
        await _columnDbContext.Columns.DeleteOneAsync(c => c.ColumnId == id);
}