using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Domain.Entities;

public class User
{
    /// <summary>
    /// Уникальный идентификатор документа.
    /// </summary>
    [BsonId]
    [NotMapped]
    public ObjectId Id { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string UserName { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Roles Role { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public List<Guid> BoardIds { get; set; } 
}
