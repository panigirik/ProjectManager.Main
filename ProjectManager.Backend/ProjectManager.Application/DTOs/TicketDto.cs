using MongoDB.Bson;

namespace ProjectManager.Application.DTOs;

public class TicketDto
{
    public Guid TicketId { get; set; } 
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid AssignedUserId { get; set; } // ID пользователя-исполнителя
    public List<Guid> AttachmentIds { get; set; } = new();
}