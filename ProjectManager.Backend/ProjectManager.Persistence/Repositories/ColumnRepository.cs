using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

public class ColumnRepository: IColumnRepository
{
    private readonly IMongoCollection<Column> _columns;

    public ColumnRepository(ColumnDbContext context)
    {
        _columns = context.Columns;
    }

    public async Task<IEnumerable<Column>> GetAllAsync() =>
        await _columns.Find(_ => true).ToListAsync();

    public async Task<Column> GetByIdAsync(string id) =>
        await _columns.Find(c => c.ColumnId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Column column) =>
        await _columns.InsertOneAsync(column);

    public async Task UpdateAsync(Column column) =>
        await _columns.ReplaceOneAsync(c => c.ColumnId == column.ColumnId, column);

    public async Task DeleteAsync(string id) =>
        await _columns.DeleteOneAsync(c => c.ColumnId == id);
}