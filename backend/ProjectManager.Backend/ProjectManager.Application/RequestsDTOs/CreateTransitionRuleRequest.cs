using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.RequestsDTOs;

public class CreateTransitionRuleRequest
{
    [BsonId]
    [NotMapped]
    public ObjectId Id { get; set; }
        
    public Guid TicketTransitionRuleId { get; set; } = Guid.NewGuid();
    
    public Guid TicketId { get; set; } 
    
    public Guid FromColumnId { get; set; }
    public Guid ToColumnId { get; set; }
    
    public bool IsAllowed { get; set; } = true;
    
    // Вместо RequiresAttachment и RequiresCommitLink используем единое поле:
    public TransitionValidationType RequiredValidations { get; set; } = TransitionValidationType.None;
    
    public Guid? UserId { get; set; } // Пользователь, который может переместить тикет
}