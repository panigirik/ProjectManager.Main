using ProjectManager.Application.DTOs;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Interfaces;

public interface IColumnService
{
    Task<IEnumerable<ColumnDto>> GetAllAsync();
    Task<ColumnDto> GetByIdAsync(Guid id);
    Task CreateAsync(ColumnDto сolumnDto);
    Task UpdateAsync(UpdateColumnRequest updateColumnRequest);
    Task DeleteAsync(Guid id);
}