using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<TicketDto>> GetAllAsync();
    Task<TicketDto> GetByIdAsync(Guid id);
    Task CreateAsync(TicketDto ticketDto);
    Task UpdateAsync(TicketDto ticketDto);
    Task DeleteAsync(Guid id);
}