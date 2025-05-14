using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Mappings;

public class RefreshTokenMappingProfile: Profile
{
    public RefreshTokenMappingProfile()
    {
        CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
    }
}