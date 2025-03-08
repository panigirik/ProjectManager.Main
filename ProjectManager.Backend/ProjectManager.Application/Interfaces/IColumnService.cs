using ProjectManager.Application.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Interfaces;

public interface IColumnService
{
    Task<IEnumerable<ColumnDto>> GetAllAsync();
    Task<ColumnDto> GetByIdAsync(string id);
    Task CreateAsync(ColumnDto сolumnDto);
    Task UpdateAsync(ColumnDto сolumnDto);
    Task DeleteAsync(string id);
}