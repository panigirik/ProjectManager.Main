using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Domain.Entities;

public class Attachment
{
    /// <summary>
    /// Уникальный идентификатор документа.
    /// </summary>
    [BsonId]
    [NotMapped]
    public ObjectId Id { get; set; } 
    
    [BsonRepresentation(BsonType.String)]
    public Guid AttachmentId { get; set; }
    
    public Guid TicketId { get; set; } 
    
    public string FileName { get; set; }
    
    public string FileUrl { get; set; }
}
