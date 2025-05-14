using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Application.RequestsDTOs;

public class UpdateColumnRequest
{
    
    [BsonRepresentation(BsonType.String)]
    public Guid ColumnId { get; set; } 
    public string ColumnName { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid BoardId { get; set; } 
    
    [BsonRepresentation(BsonType.String)]
    public List<Guid> TicketIds { get; set; } = new(); 
}