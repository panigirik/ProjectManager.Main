using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ProjectManager.Domain.Interfaces.ExternalServices;

namespace ProjectManager.ExternalServices.Services.CloudStorageServices;

public class DropBoxClient : IDropBoxClient
{
    private readonly DropboxClient _client;

    public DropBoxClient(IConfiguration configuration)
    {
        var accessToken = configuration["DropBox:AccessToken"];
        _client = new DropboxClient(accessToken);
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
        var sharedLinkMetadata = await _client.Sharing.CreateSharedLinkWithSettingsAsync(filePath);

        // Преобразуем ссылку, чтобы сразу загружался файл
        return sharedLinkMetadata.Url.Replace("?dl=0", "?dl=1");
    }

    public async Task DeleteFileAsync(string filePath)
    {
        await _client.Files.DeleteV2Async(filePath);
    }
}