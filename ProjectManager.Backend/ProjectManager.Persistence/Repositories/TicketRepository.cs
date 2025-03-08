using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

public class TicketRepository: ITicketRepository
{
    private readonly IMongoCollection<Ticket> _tickets;

    public TicketRepository(TicketDbContext context)
    {
        _tickets = context.Tickets;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync() =>
        await _tickets.Find(_ => true).ToListAsync();

    public async Task<Ticket> GetByIdAsync(string id) =>
        await _tickets.Find(t => t.TicketId == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Ticket ticket) =>
        await _tickets.InsertOneAsync(ticket);

    public async Task UpdateAsync(Ticket ticket) =>
        await _tickets.ReplaceOneAsync(t => t.TicketId == ticket.TicketId, ticket);

    public async Task DeleteAsync(string id) =>
        await _tickets.DeleteOneAsync(t => t.TicketId == id);
}