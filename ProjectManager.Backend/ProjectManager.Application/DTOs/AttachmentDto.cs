using MongoDB.Bson;

namespace ProjectManager.Application.DTOs;

public class AttachmentDto
{
    public string AttachmentId { get; set; } = ObjectId.GenerateNewId().ToString();
    
    public string TicketId { get; set; } 
    
    public string FileName { get; set; }
    
    public string FileUrl { get; set; }
}