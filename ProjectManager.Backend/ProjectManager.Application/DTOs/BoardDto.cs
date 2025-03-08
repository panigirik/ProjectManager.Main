using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Application.DTOs;

public class BoardDto
{
    public string BoardId { get; set; } = ObjectId.GenerateNewId().ToString();
    public string BoardName { get; set; }
    public List<string> UserIds { get; set; } = new();
    public List<ColumnDto> Columns { get; set; } = new();
}