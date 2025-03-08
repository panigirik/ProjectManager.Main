using MongoDB.Bson;

namespace ProjectManager.Application.DTOs;

public class TicketTransitionRuleDto
{
    public string TicketTransitionRuleId { get; set; } = ObjectId.GenerateNewId().ToString();
    public string BoardId { get; set; }
    public string FromColumnId { get; set; }
    public string ToColumnId { get; set; }
    public bool IsAllowed { get; set; } // true - разрешено, false - запрещено
}