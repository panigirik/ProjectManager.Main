using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Domain.Entities;

public class Ticket
{
    /// <summary>
    /// Уникальный идентификатор документа.
    /// </summary>
    [BsonId]
    [NotMapped]
    public ObjectId Id { get; set; } 
    
    [BsonRepresentation(BsonType.String)]
    public Guid TicketId { get; set; } 
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string AssignedUserName { get; set; } // ID пользователя-исполнителя
    
    public List<Guid>? AttachmentIds { get; set; } = new();
}
