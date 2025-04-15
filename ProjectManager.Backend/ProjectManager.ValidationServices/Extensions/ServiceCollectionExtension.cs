using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces.ValidationInterfaces;
using ProjectManager.Application.RequestsDTOs;
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
        services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();

        services.AddScoped<IFileValidationService, FileValidationService>();
        services.AddScoped<IAddUserValidationService, AddUserValidationService>();
        services.AddScoped<IUserDtoValidationService, UserDtoValidationService>();
        services.AddScoped<ILoginValidationService, LoginValidationService>();
        
        
        services.AddScoped<ScanFileForMalwareHelper>();

        }
}