using ProjectManager.Application.DTOs;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<GetTicketRequest>> GetAllAsync();
    Task<TicketDto> GetByIdAsync(Guid id);

    Task<List<TicketDto>> GetTicketsByColumnIdAsync(Guid columnId);

    Task<List<string>> GetAttachmentsPathsAsync(Guid ticketId);
    
    Task<Ticket> CreateTicketAsync(CreateTicketRequest ticketRequest);
    Task UpdateAsync(UpdateTicketRequest ticketRequest);

    Task MoveToColumnAsync(MoveTicketRequest request);
    
    Task DeleteAsync(Guid id);
}