using ProjectManager.Application.DTOs;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<GetTicketRequest>> GetAllAsync();
    Task<TicketDto> GetByIdAsync(Guid id);
    Task<Ticket> CreateTicketAsync(CreateTicketDto ticketDto);
    Task UpdateAsync(UpdateTicketDto ticketDto);

    Task MoveNextColumn(Guid oldColumnId, Guid newColumnId);
    
    Task DeleteAsync(Guid id);
}