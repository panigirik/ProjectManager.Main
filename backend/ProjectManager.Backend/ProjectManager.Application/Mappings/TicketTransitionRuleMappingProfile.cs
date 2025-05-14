using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Mappings;

public class TicketTransitionRuleMappingProfile: Profile
{
    public TicketTransitionRuleMappingProfile()
    {
        CreateMap<TicketTransitionRule, CreateTransitionRuleRequest>().ReverseMap();
        CreateMap<TicketTransitionRule, TicketTransitionRuleDto>().ReverseMap();
    }
}