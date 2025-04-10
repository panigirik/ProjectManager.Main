using System.Data;
using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.Mappings;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;

namespace ProjectManager.Application.Services
{
    public class ColumnService : IColumnService
    {
        private readonly IColumnRepository _columnRepository;
        private readonly IMapper _mapper; // Внедряем IMapper
        
        // Конструктор с внедрением IMapper
        public ColumnService(IColumnRepository columnRepository,
            IMapper mapper)
        {
            _columnRepository = columnRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ColumnDto>> GetAllAsync()
        {
            // Получаем все columns из репозитория
            var columns = await _columnRepository.GetAllAsync();
            
            // Преобразуем сущности в DTO
            return _mapper.Map<IEnumerable<ColumnDto>>(columns);
        }

        public async Task<ColumnDto> GetByIdAsync(Guid id)
        {
            // Получаем column по id
            var column = await _columnRepository.GetByIdAsync(id);
            
            // Преобразуем сущность в DTO
            return _mapper.Map<ColumnDto>(column);
        }

        public async Task CreateAsync(ColumnDto columnDto)
        {
            // Преобразуем DTO в сущность
            var column = _mapper.Map<Column>(columnDto);
            
            // Создаем column через репозиторий
            await _columnRepository.CreateAsync(column);
        }

        public async Task UpdateAsync(UpdateColumnRequest updateColumnRequest)
        {
            // Преобразуем DTO в сущность
            var column = _mapper.Map<Column>(updateColumnRequest);
            
            // Обновляем column через репозиторий
            await _columnRepository.UpdateAsync(column);
        }

        public async Task DeleteAsync(Guid id)
        {
            // Удаляем column через репозиторий
            await _columnRepository.DeleteAsync(id);
        }
    }
}
