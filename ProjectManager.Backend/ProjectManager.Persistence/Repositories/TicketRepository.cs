using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

public class TicketRepository: ITicketRepository
{
    private readonly TicketDbContext _ticketDbContext;

    public TicketRepository(TicketDbContext context)
    {
        _ticketDbContext = context;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync() =>
        await _ticketDbContext.Tickets.Find(_ => true).ToListAsync();

    public async Task<Ticket> GetByIdAsync(Guid id) =>
        await _ticketDbContext.Tickets.Find(t => t.TicketId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Ticket ticket) =>
        await _ticketDbContext.Tickets.InsertOneAsync(ticket);

    public async Task UpdateAsync(Ticket ticket)
    {
        var existing = await _ticketDbContext.Tickets
            .Find(t => t.TicketId == ticket.TicketId)
            .FirstOrDefaultAsync();

        ticket.Id = existing.Id; 

        await _ticketDbContext.Tickets.ReplaceOneAsync(t => t.TicketId == ticket.TicketId, ticket);
    }


    public async Task DeleteAsync(Guid id) =>
        await _ticketDbContext.Tickets.DeleteOneAsync(t => t.TicketId == id);
}