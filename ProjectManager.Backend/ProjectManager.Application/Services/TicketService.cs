using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;

namespace ProjectManager.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper; // Внедряем IMapper

        // Конструктор с внедрением IMapper
        public TicketService(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            // Получаем все tickets из репозитория
            var tickets = await _ticketRepository.GetAllAsync();
            
            // Преобразуем сущности в DTO
            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }

        public async Task<TicketDto> GetByIdAsync(string id)
        {
            // Получаем ticket по id
            var ticket = await _ticketRepository.GetByIdAsync(id);
            
            // Преобразуем сущность в DTO
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task CreateAsync(TicketDto ticketDto)
        {
            // Преобразуем DTO в сущность
            var ticket = _mapper.Map<Ticket>(ticketDto);
            
            // Создаем ticket через репозиторий
            await _ticketRepository.CreateAsync(ticket);
        }

        public async Task UpdateAsync(TicketDto ticketDto)
        {
            // Преобразуем DTO в сущность
            var ticket = _mapper.Map<Ticket>(ticketDto);
            
            // Обновляем ticket через репозиторий
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task DeleteAsync(string id)
        {
            // Удаляем ticket через репозиторий
            await _ticketRepository.DeleteAsync(id);
        }
    }
}
