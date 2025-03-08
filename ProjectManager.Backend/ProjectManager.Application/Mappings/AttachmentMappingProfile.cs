using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Mappings;

public class AttachmentMappingProfile: Profile
{
    public AttachmentMappingProfile()
    {
        CreateMap<AttachmentDto, Attachment>();

        CreateMap<Attachment, AttachmentDto>();
    }
}