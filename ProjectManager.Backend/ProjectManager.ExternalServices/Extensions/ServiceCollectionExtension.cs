using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Domain.Interfaces.ExternalServices;
using ProjectManager.ExternalServices.Services.CloudStorageServices;

namespace ProjectManager.ExternalServices.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureExternalServices(this IServiceCollection services)
    {
        services.AddScoped<IDropBoxClient, DropBoxClient>(); 
            
        //services.AddScoped<IUploadService, UploadService>();
    }
}

