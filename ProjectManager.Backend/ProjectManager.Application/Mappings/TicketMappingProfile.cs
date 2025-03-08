using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Mappings;

public class TicketMappingProfile: Profile
{
    /// <summary>
    /// Конструктор маппинга между  ApplicationUser и ApplicationUserDTO.
    /// </summary>
    public TicketMappingProfile()
    {
        CreateMap<TicketDto, Ticket>();

        CreateMap<Ticket, TicketDto>();
    }
}