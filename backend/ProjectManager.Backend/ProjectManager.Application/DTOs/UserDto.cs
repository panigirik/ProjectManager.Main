using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.DTOs;

public class UserDto
{
    [BsonRepresentation(BsonType.String)] 
    public Guid UserId { get; set; } = Guid.NewGuid();
    
    public string UserName { get; set; }
    public string Email { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Roles Role { get; set; }
    
    public string Password { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public List<Guid> BoardIds { get; set; } = new(); 
}