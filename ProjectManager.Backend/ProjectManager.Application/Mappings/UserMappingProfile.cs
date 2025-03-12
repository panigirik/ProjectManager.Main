using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Mappings;

public class UserMappingProfile: Profile
{
    /// <summary>
    /// Конструктор маппинга между  ApplicationUser и ApplicationUserDTO.
    /// </summary>
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<UserDto, User>();
    }
}