using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Domain.Entities;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = ObjectId.GenerateNewId().ToString();
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public List<string> BoardIds { get; set; } = new(); 
}
