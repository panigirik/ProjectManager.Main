using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Application.DTOs;

public class UserDto
{
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; } 
    public string UserName { get; set; }
    public string Email { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public List<Guid> BoardIds { get; set; } = new(); 
}