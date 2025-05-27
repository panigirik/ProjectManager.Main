using AutoMapper;
using Netway.Utils.Interfaces;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;

namespace ProjectManager.Application.Services;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;
    private readonly IMapper _mapper;
    private readonly ITicketRepository _ticketRepository;
    private readonly IColumnRepository _columnRepository;
    private readonly IUserHelperService _userHelperService;
    
    public BoardService(
        IBoardRepository boardRepository,
        IMapper mapper,
        ITicketRepository ticketRepository,
        IColumnRepository columnRepository,
        IUserHelperService userHelperService)
    {
        _boardRepository = boardRepository;
        _mapper = mapper;
        _ticketRepository = ticketRepository;
        _columnRepository = columnRepository;
        _userHelperService = userHelperService;
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
        boardDto.BoardId = Guid.NewGuid();
        boardDto.CreatorId = _userHelperService.GetCurrentUser().UserId;
        var board = _mapper.Map<Board>(boardDto);
        await _boardRepository.CreateAsync(board);
    }

    public async Task UpdateAsync(BoardDto boardDto)
    {
        var updatedBoard = _mapper.Map<Board>(boardDto);
        updatedBoard.CreatorId = _userHelperService.GetCurrentUser().UserId;
        await _boardRepository.UpdateAsync(updatedBoard);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _boardRepository.DeleteAsync(id);
    }
    
    public async Task<BoardDto?> GetBoardByTicketIdAsync(Guid ticketId)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        if (ticket == null)
            return null;

        var column = await _columnRepository.GetByIdAsync(ticket.ColumnId);
        if (column == null)
            return null;

        var board = await _boardRepository.GetByIdAsync(column.BoardId);
        if (board == null)
            return null;

        return _mapper.Map<BoardDto>(board);
    }

    
}