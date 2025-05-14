using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Application.DTOs;

public class TicketTransitionRuleDto
{
    [BsonId]
    [NotMapped]
    public ObjectId Id { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid TicketTransitionRuleId { get; set; } = Guid.NewGuid();

    [BsonRepresentation(BsonType.String)]
    public Guid TicketId { get; set; } 

    [BsonRepresentation(BsonType.String)]
    public Guid FromColumnId { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid ToColumnId { get; set; }

    public bool IsAllowed { get; set; } = true;

    public bool RequiresAttachment { get; set; } = false;
    public bool RequiresCommitLink { get; set; } = false;

    [BsonRepresentation(BsonType.String)]
    public Guid? UserId { get; set; } // Пользователь, который может переместить тикет
}