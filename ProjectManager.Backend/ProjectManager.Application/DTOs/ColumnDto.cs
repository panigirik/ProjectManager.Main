using MongoDB.Bson;

namespace ProjectManager.Application.DTOs;

public class ColumnDto
{
    public Guid ColumnId { get; set; } 
    public string ColumnName { get; set; }
    public List<TicketDto> Tickets { get; set; } = new();
}