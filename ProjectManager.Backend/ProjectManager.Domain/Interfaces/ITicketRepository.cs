using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interfaces;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task<Ticket> GetByIdAsync(string id);
    Task CreateAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
    Task DeleteAsync(string id);
}