using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interfaces;

public interface ITicketTransitionRuleRepository
{
    Task<TicketTransitionRule?> GetRuleForTicketAsync(Guid ticketId, Guid fromColumnId, Guid toColumnId);
    
    Task<TicketTransitionRule> AddTransitionRuleAsync(TicketTransitionRule ticketTransitionRule);
}
