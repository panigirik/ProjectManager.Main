using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectManager.Application.RequestsDTOs;

public class MoveTicketRequest
{
    [BsonRepresentation(BsonType.String)]
    public Guid TicketId { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid NewColumnId { get; set; }

    // Опционально, если правило требует commit-ссылку
    public string? CommitLink { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    // Опционально, если правило требует файл (или файлы)
    public IFormFile[]? Attachments { get; set; }
}