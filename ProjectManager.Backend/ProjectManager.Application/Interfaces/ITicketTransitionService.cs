using ProjectManager.Application.DTOs;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Interfaces;

public interface ITicketTransitionService
{
    Task ValidateTransitionAsync(TicketDto ticketDto, Guid toColumnId, Guid currentUserId);

    Task<TicketTransitionRule> AddTransitionRuleAsync(CreateTransitionRuleRequest request);

}
