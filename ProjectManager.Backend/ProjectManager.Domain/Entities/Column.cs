using MongoDB.Bson;

namespace ProjectManager.Domain.Entities;

public class Column
{
    public string ColumnId { get; set; } = ObjectId.GenerateNewId().ToString();
    public string ColumnName { get; set; }
    public List<Ticket> Tickets { get; set; } = new();
}
