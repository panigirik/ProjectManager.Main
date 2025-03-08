using MongoDB.Bson;

namespace ProjectManager.Application.DTOs;

public class ColumnDto
{
    public string ColumnId { get; set; } = ObjectId.GenerateNewId().ToString();
    public string ColumnName { get; set; }
    public List<TicketDto> Tickets { get; set; } = new();
}