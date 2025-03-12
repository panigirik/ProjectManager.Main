using AutoMapper;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;

namespace ProjectManager.Application.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IMapper _mapper; // Внедряем IMapper

        // Конструктор с внедрением IMapper
        public AttachmentService(IAttachmentRepository attachmentRepository, IMapper mapper)
        {
            _attachmentRepository = attachmentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AttachmentDto>> GetAllAsync()
        {
            // Получаем все attachments из репозитория
            var attachments = await _attachmentRepository.GetAllAsync();
            
            // Преобразуем сущности в DTO
            return _mapper.Map<IEnumerable<AttachmentDto>>(attachments);
        }

        public async Task<AttachmentDto> GetByIdAsync(Guid id)
        {
            // Получаем attachment по id
            var attachment = await _attachmentRepository.GetByIdAsync(id);
            
            // Преобразуем сущность в DTO
            return _mapper.Map<AttachmentDto>(attachment);
        }

        public async Task CreateAsync(AttachmentDto attachmentDto)
        {
            // Преобразуем DTO в сущность
            var attachment = _mapper.Map<Attachment>(attachmentDto);
            
            // Создаем attachment через репозиторий
            await _attachmentRepository.CreateAsync(attachment);
        }

        public async Task UpdateAsync(AttachmentDto attachmentDto)
        {
            // Преобразуем DTO в сущность
            var attachment = _mapper.Map<Attachment>(attachmentDto);
            
            // Обновляем attachment через репозиторий
            await _attachmentRepository.UpdateAsync(attachment);
        }

        public async Task DeleteAsync(Guid id)
        {
            // Удаляем attachment через репозиторий
            await _attachmentRepository.DeleteAsync(id);
        }
    }
}
