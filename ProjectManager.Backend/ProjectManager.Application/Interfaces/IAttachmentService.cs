using ProjectManager.Application.DTOs;

namespace ProjectManager.Application.Interfaces;

public interface IAttachmentService
{
    Task<IEnumerable<AttachmentDto>> GetAllAsync();
    Task<AttachmentDto> GetByIdAsync(string id);
    Task CreateAsync(AttachmentDto attachment);
    Task UpdateAsync(AttachmentDto attachment);
    Task DeleteAsync(string id);
}