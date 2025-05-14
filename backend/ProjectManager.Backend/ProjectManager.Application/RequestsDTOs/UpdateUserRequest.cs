using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Application.RequestsDTOs;

public class UpdateUserRequest
{
    [BsonRepresentation(BsonType.String)] 
    public Guid UserId { get; set; } = Guid.NewGuid();
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    
}