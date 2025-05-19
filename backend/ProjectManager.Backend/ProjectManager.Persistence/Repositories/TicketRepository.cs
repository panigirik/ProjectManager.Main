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

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _ticketDbContext.Tickets.Find(_ => true).ToListAsync();
    }


    public async Task<Ticket> GetByIdAsync(Guid ticketId)
    {
        return await _ticketDbContext.Tickets.Find(t => t.TicketId == ticketId).FirstOrDefaultAsync();
    }
       

    public async Task<IEnumerable<Ticket>> GetTicketsByColumnId(Guid columnId)
    {
        return await _ticketDbContext.Tickets.Find(t => t.ColumnId == columnId).ToListAsync();
    }


    public async Task CreateAsync(Ticket ticket)
    {
        await _ticketDbContext.Tickets.InsertOneAsync(ticket);
    }
        

    public async Task UpdateAsync(Ticket ticket)
    {
        var existing = await _ticketDbContext.Tickets
            .Find(t => t.TicketId == ticket.TicketId)
            .FirstOrDefaultAsync();

        ticket.Id = existing.Id; 

        await _ticketDbContext.Tickets.ReplaceOneAsync(t => t.TicketId == ticket.TicketId, ticket);
    }


    public async Task DeleteAsync(Guid id)
    {
        await _ticketDbContext.Tickets.DeleteOneAsync(t => t.TicketId == id);
    }
    
}