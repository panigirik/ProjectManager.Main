using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Mappings;

public class BoardMappingProfile: Profile
{
    public BoardMappingProfile()
    {
        CreateMap<BoardDto, Board>();

        CreateMap<Board, BoardDto>();
    }
}