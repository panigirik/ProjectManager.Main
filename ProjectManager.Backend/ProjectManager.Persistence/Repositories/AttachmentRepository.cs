using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

public class AttachmentRepository: IAttachmentRepository
{
    private readonly AttachmentDbContext _attachmentDbContext;

        public AttachmentRepository(AttachmentDbContext context)
        {
            _attachmentDbContext = context;
        }

        public async Task<IEnumerable<Attachment>> GetAllAsync() =>
            await _attachmentDbContext.Attachments.Find(_ => true).ToListAsync();

        public async Task<Attachment> GetByIdAsync(string id) =>
            await _attachmentDbContext.Attachments.Find(a => a.AttachmentId == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Attachment attachment) =>
            await _attachmentDbContext.Attachments.InsertOneAsync(attachment);

        public async Task UpdateAsync(Attachment attachment) =>
            await _attachmentDbContext.Attachments.ReplaceOneAsync(a => a.AttachmentId == attachment.AttachmentId, attachment);

        public async Task DeleteAsync(string id) =>
            await _attachmentDbContext.Attachments.DeleteOneAsync(a => a.AttachmentId == id);
    
}