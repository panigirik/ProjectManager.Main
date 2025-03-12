using MongoDB.Bson;

namespace ProjectManager.Domain.Entities;

public class TicketTransitionRule
{
    public Guid TicketTransitionRuleId { get; set; }
    public Guid BoardId { get; set; }
    public Guid FromColumnId { get; set; }
    public Guid ToColumnId { get; set; }
    public bool IsAllowed { get; set; } // true - разрешено, false - запрещено
}
