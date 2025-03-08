using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Persistence.Data;
using ProjectManager.Persistence.Repositories;

namespace ProjectManager.Persistence.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureRepositoriesServices(this IServiceCollection services)
    {
        services.AddSingleton<AttachmentDbContext>();
        services.AddSingleton<UserDbContext>();
        services.AddSingleton<BoardDbContext>();
        services.AddSingleton<ColumnDbContext>();
        services.AddScoped<TicketDbContext>();

        //services.AddScoped<IUserRepository, UserRepository>(); репозитории регнуть
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IColumnRepository, ColumnRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

    }
}