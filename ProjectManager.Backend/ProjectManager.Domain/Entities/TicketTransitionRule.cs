using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Domain.Entities;

public class TicketTransitionRule
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
    
    // Универсальное поле валидации перехода
    public TransitionValidationType RequiredValidations { get; set; } = TransitionValidationType.None;
    
    [BsonRepresentation(BsonType.String)]
    public Guid? UserId { get; set; } // Пользователь, который может переместить тикет
}
