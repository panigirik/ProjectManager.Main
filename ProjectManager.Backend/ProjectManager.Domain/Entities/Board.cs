using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Domain.Entities;

public class Board
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string BoardId { get; set; } = ObjectId.GenerateNewId().ToString();

    public string BoardName { get; set; }
    public List<string> UserIds { get; set; } = new();
    public List<Column> Columns { get; set; } = new();
}
