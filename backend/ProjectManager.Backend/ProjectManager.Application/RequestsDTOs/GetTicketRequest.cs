using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Application.RequestsDTOs;

public class GetTicketRequest
{
    [BsonRepresentation(BsonType.String)]
    public Guid TicketId { get; set; } 
    public string Title { get; set; }
    public string Description { get; set; }
    public string AssignedUserName { get; set; } 
    
    [BsonRepresentation(BsonType.String)]
    public Guid ColumnId { get; set; } 
    
    /// <summary>
    /// Прикрепленные ссылки на файлы к сообщению.
    /// </summary>
    public string[]? Attachments { get; set; }
}