using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interfaces;

public interface IAttachmentRepository
{
    Task<IEnumerable<Attachment>> GetAllAsync();
    Task<Attachment> GetByIdAsync(string id);
    Task CreateAsync(Attachment attachment);
    Task UpdateAsync(Attachment attachment);
    Task DeleteAsync(string id);
}