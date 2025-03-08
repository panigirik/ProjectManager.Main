using MongoDB.Bson;

namespace ProjectManager.Application.DTOs;

public class TicketDto
{
    public string TicketId { get; set; } = ObjectId.GenerateNewId().ToString();
    public string Title { get; set; }
    public string Description { get; set; }
    public string AssignedUserId { get; set; } // ID пользователя-исполнителя
    public List<string> AttachmentIds { get; set; } = new();
}