using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;

namespace ProjectManager.Application.Services;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;
    private readonly IMapper _mapper;

    public BoardService(IBoardRepository boardRepository, IMapper mapper)
    {
        _boardRepository = boardRepository;
        _mapper = mapper;

    }

    public async Task<IEnumerable<BoardDto>> GetAllAsync()
    {
        var boards = await _boardRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<BoardDto>>(boards);
    }

    public async Task<BoardDto> GetByIdAsync(Guid id)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        return _mapper.Map<BoardDto>(board);
    }

    public async Task CreateAsync(BoardDto boardDto)
    {
        var board = _mapper.Map<Board>(boardDto);
        await _boardRepository.CreateAsync(board);
    }

    public async Task UpdateAsync(BoardDto boardDto)
    {
        var updatedBoard = _mapper.Map<Board>(boardDto);
        
        await _boardRepository.UpdateAsync(updatedBoard);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _boardRepository.DeleteAsync(id);
    }
}