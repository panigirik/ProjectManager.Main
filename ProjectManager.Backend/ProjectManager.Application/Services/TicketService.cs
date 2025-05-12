using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.RequestsDTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Interfaces.ExternalServices;

namespace ProjectManager.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly IDropBoxClient _dropBoxClient;
        private readonly ITicketTransitionRuleRepository _ticketTransitionRuleRepository;

        public TicketService(ITicketRepository ticketRepository,
            IMapper mapper,
            IDropBoxClient dropBoxClient,
            ITicketTransitionRuleRepository ticketTransitionRuleRepository)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _dropBoxClient = dropBoxClient;
            _ticketTransitionRuleRepository = ticketTransitionRuleRepository;
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

        public async Task<List<TicketDto>> GetTicketsByColumnIdAsync(Guid columnId)
        {
            var tickets = await _ticketRepository.GetTicketsByColumnId(columnId);
            return _mapper.Map<List<TicketDto>>(tickets);
        }

        
        public async Task<Ticket> CreateTicketAsync(CreateTicketRequest ticketRequest)
        {
            var uploadedUrls = new List<string>();

            if (ticketRequest.Attachments != null && ticketRequest.Attachments.Length > 0)
            {
                foreach (var file in ticketRequest.Attachments)
                {
                    var dropboxPath = $"/Documents/{file.FileName}";

                    var uploadedUrl = await _dropBoxClient.UploadFileAsync(file, dropboxPath);
                    uploadedUrls.Add(uploadedUrl);
                }
            }
            
            var ticket = _mapper.Map<Ticket>(ticketRequest);
            ticket.Attachments = uploadedUrls.ToArray();
            await _ticketRepository.CreateAsync(ticket);
            return ticket;
        }



        public async Task UpdateAsync(UpdateTicketRequest ticketRequest)
        {
            var ticket = _mapper.Map<Ticket>(ticketRequest);
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task MoveToColumnAsync(MoveTicketRequest request)
        {
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId)
                             ?? throw new InvalidOperationException("Ticket not found.");

            var fromColumnId = ticket.ColumnId;

            var rule = await _ticketTransitionRuleRepository.GetRuleForTicketAsync(
                ticket.TicketId, fromColumnId, request.NewColumnId);

            if (rule != null)
            {
                if (!rule.IsAllowed)
                    throw new InvalidOperationException("Transition is not allowed.");
                
                var validations = rule.RequiredValidations;

                if (validations != TransitionValidationType.None)
                {
                    bool hasAttachment = (request.Attachments?.Length > 0) ||
                                         (ticket.Attachments?.Length > 0);

                    bool hasCommitLink = !string.IsNullOrWhiteSpace(request.CommitLink) ||
                                         (!string.IsNullOrWhiteSpace(ticket.Description) &&
                                          ticket.Description.Contains("http"));

                    if (validations.HasFlag(TransitionValidationType.Attachment) && !hasAttachment)
                        throw new InvalidOperationException("Transition requires an attachment.");

                    if (validations.HasFlag(TransitionValidationType.CommitLink) && !hasCommitLink)
                        throw new InvalidOperationException("Transition requires a commit link.");
                }

                if (rule.UserId.HasValue && rule.UserId.Value != request.UserId)
                    throw new InvalidOperationException("Only the assigned user can move this ticket.");
            }
            
            if (!string.IsNullOrWhiteSpace(request.CommitLink))
            {
                ticket.Description = request.CommitLink;
            }
            
            if (request.Attachments != null && request.Attachments.Length > 0)
            {
                var uploadedUrls = new List<string>();
                foreach (var file in request.Attachments)
                {
                    var dropboxPath = $"/Documents/{file.FileName}";
                    var uploadedUrl = await _dropBoxClient.UploadFileAsync(file, dropboxPath);
                    uploadedUrls.Add(uploadedUrl);
                }

                ticket.Attachments = uploadedUrls.ToArray();
            }
            
            ticket.ColumnId = request.NewColumnId;
            await _ticketRepository.UpdateAsync(ticket);
        }


        public async Task DeleteAsync(Guid id)
        {
            await _ticketRepository.DeleteAsync(id);
        }
    }
}
