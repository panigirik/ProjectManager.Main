using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ProjectManager.Domain.Interfaces.ExternalServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ProjectManager.Domain.Interfaces;

namespace ProjectManager.ExternalServices.Services.CloudStorageServices
{
    public class DropBoxClient : IDropBoxClient
    {
        private readonly DropboxClient _client;
        private readonly ITicketRepository _ticketRepository;

        public DropBoxClient(IConfiguration configuration,
            ITicketRepository ticketRepository)
        {
            var accessToken = configuration["DropBox:AccessToken"];
            _client = new DropboxClient(accessToken);
            _ticketRepository = ticketRepository;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string filePath)
        {
            using var stream = file.OpenReadStream();

            await _client.Files.UploadAsync(
                path: filePath,
                mode: WriteMode.Overwrite.Instance,
                body: stream);

            return await GetFileLinkAsync(filePath);
        }

        public async Task<string> GetFileLinkAsync(string filePath)
        {
            var temporalyLink = await _client.Files.GetTemporaryLinkAsync(filePath);
            return temporalyLink.Link;
        }

        public async Task DeleteFileAsync(string filePath)
        {
            await _client.Files.DeleteV2Async(filePath);
        }
        
        public async Task<List<string>> GetAttachmentsPathsAsync(Guid ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
                throw new ArgumentException($"Тикет с Id {ticketId} не найден.");

            var attachmentsPaths = ticket.Attachments;
            if (attachmentsPaths == null || !attachmentsPaths.Any())
                return new List<string>();

            // Просто возвращаем ссылки из базы, без вызова Dropbox API
            return attachmentsPaths;
        }

        
        
        
    }
}
