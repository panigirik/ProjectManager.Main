﻿using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interfaces;

public interface IColumnRepository
{
    Task<IEnumerable<Column>> GetAllAsync();
    Task<Column> GetByIdAsync(Guid id);

    Task<IEnumerable<Column>> GetColumnsByBoardAsync(Guid boardId);
    Task CreateAsync(Column column);
    Task UpdateAsync(Column column);
    Task DeleteAsync(Guid id);
}