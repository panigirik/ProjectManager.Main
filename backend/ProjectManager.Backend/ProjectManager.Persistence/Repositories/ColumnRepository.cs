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

    public async Task<IEnumerable<Column>> GetAllAsync()
    {
        return await _columnDbContext.Columns.Find(_ => true).ToListAsync();
    }


    public async Task<Column> GetByIdAsync(Guid id)
    {
        return await _columnDbContext.Columns.Find(c => c.ColumnId == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Column>> GetColumnsByBoardAsync(Guid boardId)
    {
        return await _columnDbContext.Columns.Find(c => c.BoardId == boardId).ToListAsync();
    }

    public async Task CreateAsync(Column column)
    {
        await _columnDbContext.Columns.InsertOneAsync(column);
    }
        

    public async Task UpdateAsync(Column column)
    {
        var update = Builders<Column>.Update
            .Set(c => c.ColumnId, column.ColumnId)
            .Set(c => c.TicketIds, column.TicketIds)
            .Set(c => c.BoardId, column.BoardId);
        
        var result = await _columnDbContext.Columns.UpdateOneAsync(
            c => c.ColumnId == column.ColumnId,
            update,
            cancellationToken: CancellationToken.None
        );
        

        
        if (result.MatchedCount == 0)
            throw new InvalidOperationException("Column with specified ColumnId not found.");

        if (result.ModifiedCount == 0)
            Console.WriteLine("Document matched, but no fields were changed.");
        
    }

        

    public async Task DeleteAsync(Guid id) =>
        await _columnDbContext.Columns.DeleteOneAsync(c => c.ColumnId == id);
}