using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Application.DTOs;

public class BoardDto
{
    [BsonRepresentation(BsonType.String)]
    public Guid BoardId { get; set; } 
    public string BoardName { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid CreatorId { get; set; }

    [BsonRepresentation(BsonType.String)]
    public List<Guid> ColumnIds { get; set; }
}