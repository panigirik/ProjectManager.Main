using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;
using ProjectManager.Domain.Interfaces;

namespace ProjectManager.Application.Services;

public class TicketTransitionService : ITicketTransitionService
{
    private readonly ITicketTransitionRuleRepository _ruleRepository;

    public TicketTransitionService(ITicketTransitionRuleRepository ruleRepository)
    {
        _ruleRepository = ruleRepository;
    }

        public async Task ValidateTransitionAsync(TicketDto ticketDto, Guid toColumnId, Guid currentUserId)
        {
            var fromColumnId = ticketDto.ColumnId;
            var rule = await _ruleRepository.GetRuleForTicketAsync(ticketDto.TicketId, fromColumnId, toColumnId);
    
            if (rule == null)
                return;
    
            if (!rule.IsAllowed)
                throw new InvalidOperationException("Transition is not allowed.");
            
            if (rule.RequiredValidations != TransitionValidationType.None)
            {
                bool hasAttachment = ticketDto.Attachments != null && ticketDto.Attachments.Length > 0;
                bool hasCommitLink = !string.IsNullOrWhiteSpace(ticketDto.Description) && ticketDto.Description.Contains("http");
                
                if (rule.RequiredValidations == TransitionValidationType.Attachment && !hasAttachment)
                {
                    throw new InvalidOperationException("Transition requires an attachment.");
                }
                if (rule.RequiredValidations == TransitionValidationType.CommitLink && !hasCommitLink)
                {
                    throw new InvalidOperationException("Transition requires a commit link in the description.");
                }
                if (rule.RequiredValidations == (TransitionValidationType.Attachment | TransitionValidationType.CommitLink)
                    && (!hasAttachment || !hasCommitLink))
                {
                    throw new InvalidOperationException("Transition requires both an attachment and a commit link.");
                }
            }
    
            if (rule.UserId.HasValue && rule.UserId.Value != currentUserId)
                throw new InvalidOperationException("Only the assigned user can move this ticket.");
        }
        
        public async Task<TicketTransitionRule> AddTransitionRuleAsync(CreateTransitionRuleRequest request)
        {
            var rule = new TicketTransitionRule
            {
                TicketTransitionRuleId = request.TicketTransitionRuleId,
                TicketId = request.TicketId,
                FromColumnId = request.FromColumnId,
                ToColumnId = request.ToColumnId,
                IsAllowed = request.IsAllowed,
                RequiredValidations = request.RequiredValidations,
                UserId = request.UserId
            };
            
            return await _ruleRepository.AddTransitionRuleAsync(rule);
        }
        
    }
    


    

