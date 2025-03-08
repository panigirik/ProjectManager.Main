using MongoDB.Driver;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;

namespace ProjectManager.Persistence.Repositories;

public class AttachmentRepository: IAttachmentRepository
{
        private readonly IMongoCollection<Attachment> _attachments;

        public AttachmentRepository(AttachmentDbContext context)
        {
            _attachments = context.Attachments;
        }

        public async Task<IEnumerable<Attachment>> GetAllAsync() =>
            await _attachments.Find(_ => true).ToListAsync();

        public async Task<Attachment> GetByIdAsync(string id) =>
            await _attachments.Find(a => a.AttachmentId == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Attachment attachment) =>
            await _attachments.InsertOneAsync(attachment);

        public async Task UpdateAsync(Attachment attachment) =>
            await _attachments.ReplaceOneAsync(a => a.AttachmentId == attachment.AttachmentId, attachment);

        public async Task DeleteAsync(string id) =>
            await _attachments.DeleteOneAsync(a => a.AttachmentId == id);
    
}