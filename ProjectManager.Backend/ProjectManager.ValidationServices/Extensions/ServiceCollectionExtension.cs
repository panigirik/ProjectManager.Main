using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Application.Interfaces.ValidationInterfaces;
using ProjectManager.Application.ValidationInterfaces;
using ProjectManager.ExternalServices.Services.ClamAV.Helpers;
using ProjectManager.ValidationServices.Services;
using ProjectManager.ValidationServices.ValidateRules;

namespace ProjectManager.ValidationServices.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureValidationServices(this IServiceCollection services)
    {
        services.AddScoped<IValidator<IFormFile>, FileFormValidator>();

        services.AddScoped<IFileValidationService, FileValidationService>();
        
        services.AddScoped<ScanFileForMalwareHelper>();
    }
}