using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.RequestsDTOs;
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
            //.ForMember(dest 
               // => dest.Attachments, opt => opt.Ignore());

        CreateMap<Ticket, GetTicketRequest>();

        CreateMap<Ticket, CreateTicketRequest>().ReverseMap();

        CreateMap<Ticket, UpdateTicketRequest>().ReverseMap();
    }
}