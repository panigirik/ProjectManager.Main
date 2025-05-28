using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;

namespace ProjectManager.Application.Services
{
    public class ColumnService : IColumnService
    {
        private readonly IColumnRepository _columnRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IMapper _mapper;
        
        public ColumnService(IColumnRepository columnRepository,
            IBoardRepository boardRepository,
            IMapper mapper)
        {
            _columnRepository = columnRepository;
            _boardRepository = boardRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ColumnDto>> GetAllAsync()
        {
            var columns = await _columnRepository.GetAllAsync();
            
            return _mapper.Map<IEnumerable<ColumnDto>>(columns);
        }

        public async Task<ColumnDto> GetByIdAsync(Guid id)
        {
            var column = await _columnRepository.GetByIdAsync(id);
            
            return _mapper.Map<ColumnDto>(column);
        }

        public async Task<IEnumerable<ColumnDto>> GetColumnsByBoardIdAsync(Guid boardId)
        {
            var columns = await _columnRepository.GetColumnsByBoardAsync(boardId);
            return _mapper.Map<IEnumerable<ColumnDto>>(columns);
        }

        public async Task CreateAsync(ColumnDto columnDto)
        {
            columnDto.ColumnId = Guid.NewGuid();
            var column = _mapper.Map<Column>(columnDto);
            await _columnRepository.CreateAsync(column);
            
            var board = await _boardRepository.GetByIdAsync(column.BoardId);
            if (board == null)
                throw new InvalidOperationException("Board not found for column.");

            board.ColumnIds.Add(column.ColumnId);
            await _boardRepository.UpdateAsync(board);
        }


        public async Task UpdateAsync(UpdateColumnRequest updateColumnRequest)
        {
            var column = _mapper.Map<Column>(updateColumnRequest);
            
            await _columnRepository.UpdateAsync(column);
        }

        public async Task DeleteAsync(Guid columnId)
        {
            var column = await _columnRepository.GetByIdAsync(columnId);
            if (column == null)
                throw new InvalidOperationException("Column not found.");

            await _columnRepository.DeleteAsync(columnId);

            // Удалим колонку из доски
            var board = await _boardRepository.GetByIdAsync(column.BoardId);
            if (board != null)
            {
                board.ColumnIds.Remove(columnId);
                await _boardRepository.UpdateAsync(board);
            }
        }

        
        
    }
}
