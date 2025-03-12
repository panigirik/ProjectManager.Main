using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Application.DTOs;

public class BoardDto
{
    public Guid BoardId { get; set; } 
    public string BoardName { get; set; }
    public List<Guid> UserIds { get; set; } = new();
    public List<ColumnDto> Columns { get; set; } = new();
}