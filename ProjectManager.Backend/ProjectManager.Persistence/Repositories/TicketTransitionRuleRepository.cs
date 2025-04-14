using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

public class TicketTransitionRuleRepository : ITicketTransitionRuleRepository
{
    private readonly TicketTransitionRuleDbContext _context;

    public TicketTransitionRuleRepository(TicketTransitionRuleDbContext context)
    {
        _context = context;
    }

    public async Task<TicketTransitionRule?> GetRuleForTicketAsync(Guid ticketId, Guid fromColumnId, Guid toColumnId)
    {
        var filter = Builders<TicketTransitionRule>.Filter.And(
            Builders<TicketTransitionRule>.Filter.Eq(r => r.TicketId, ticketId),
            Builders<TicketTransitionRule>.Filter.Eq(r => r.FromColumnId, fromColumnId),
            Builders<TicketTransitionRule>.Filter.Eq(r => r.ToColumnId, toColumnId)
        );

        return await _context.TicketTransitionRules.Find(filter).FirstOrDefaultAsync();
    }
    
    public async Task<TicketTransitionRule> AddTransitionRuleAsync(TicketTransitionRule rule)
    {
        await _context.TicketTransitionRules.InsertOneAsync(rule);
        return rule;
    }

    
}