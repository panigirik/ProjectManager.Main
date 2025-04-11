using Microsoft.Extensions.DependencyInjection;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Interfaces.Jwt;
using ProjectManager.Persistence.Data;
using ProjectManager.Persistence.Repositories;
using ProjectManager.Persistence.Services;

namespace ProjectManager.Persistence.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureRepositoriesServices(this IServiceCollection services)
    {
        services.AddSingleton<UserDbContext>();
        services.AddSingleton<BoardDbContext>();
        services.AddSingleton<ColumnDbContext>();
        services.AddSingleton<RefreshTokenDbContext>();
        services.AddScoped<TicketDbContext>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IColumnRepository, ColumnRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
    }
}