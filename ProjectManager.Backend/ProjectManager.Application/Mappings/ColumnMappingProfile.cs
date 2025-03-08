using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Mappings;

public class ColumnMappingProfile: Profile
{
    public ColumnMappingProfile()
    {
        CreateMap<Column, ColumnDto>();

        CreateMap<ColumnDto, Column>();
    }
}