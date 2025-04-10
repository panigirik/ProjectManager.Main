using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.Interfaces.ExternalServices;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;

namespace ProjectManager.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly IDropBoxClient _dropBoxClient;

        // Конструктор с внедрением IMapper
        public TicketService(ITicketRepository ticketRepository,
            IMapper mapper,
            IDropBoxClient dropBoxClient)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _dropBoxClient = dropBoxClient;
        }

        public async Task<IEnumerable<GetTicketRequest>> GetAllAsync()
        { 
            var tickets = await _ticketRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetTicketRequest>>(tickets);
        }

        public async Task<TicketDto> GetByIdAsync(Guid id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            
            return _mapper.Map<TicketDto>(ticket);
        }

        public async Task<Ticket> CreateTicketAsync(CreateTicketDto ticketDto)
        {
            var uploadedUrls = new List<string>();

            if (ticketDto.Attachments != null && ticketDto.Attachments.Length > 0)
            {
                foreach (var file in ticketDto.Attachments)
                {
                    var dropboxPath = $"/Documents/{file.FileName}";

                    var uploadedUrl = await _dropBoxClient.UploadFileAsync(file, dropboxPath);
                    uploadedUrls.Add(uploadedUrl);
                }
            }
            
            var ticket = _mapper.Map<Ticket>(ticketDto);
            ticket.Attachments = uploadedUrls.ToArray();
            await _ticketRepository.CreateAsync(ticket);
            return ticket;
        }



        public async Task UpdateAsync(UpdateTicketDto ticketDto)
        {
            var ticket = _mapper.Map<Ticket>(ticketDto);
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task MoveNextColumn(Guid oldColumnId, Guid newColumnId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(oldColumnId);
            if (ticket == null)
            {
                throw new InvalidOperationException("Ticket not found.");
            }

            ticket.ColumnId = newColumnId; 

            await _ticketRepository.UpdateAsync(ticket);
        }


        public async Task DeleteAsync(Guid id)
        {
            await _ticketRepository.DeleteAsync(id);
        }
    }
}
