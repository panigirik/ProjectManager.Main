using MongoDB.Bson;

namespace ProjectManager.Application.DTOs;

public class AttachmentDto
{
    public Guid AttachmentId { get; set; } 
    
    public Guid TicketId { get; set; } 
    
    public string FileName { get; set; }
    
    public string FileUrl { get; set; }
}