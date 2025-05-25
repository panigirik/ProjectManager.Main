using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Interfaces;

public interface IBoardService
{
    Task<IEnumerable<BoardDto>> GetAllAsync();
    Task<BoardDto> GetByIdAsync(Guid id);
    Task CreateAsync(BoardDto boardDto);
    Task UpdateAsync(BoardDto boardDto);
    Task DeleteAsync(Guid id);

    Task<BoardDto?> GetBoardByTicketIdAsync(Guid ticketId);
}