﻿
using Microsoft.AspNetCore.Http;

namespace ProjectManager.Domain.Interfaces.ExternalServices;

public interface IDropBoxClient
{
    Task<string> UploadFileAsync(IFormFile file, string filePath);
    Task<string> GetFileLinkAsync(string filePath);

    Task<List<string>> GetAttachmentsPathsAsync(Guid ticketId);
    
    Task DeleteFileAsync(string filePath);
}