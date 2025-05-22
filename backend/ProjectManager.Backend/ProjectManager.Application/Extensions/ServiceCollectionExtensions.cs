using Microsoft.Extensions.DependencyInjection;
using Netway.Utils.Interfaces;
using Netway.Utils.Services;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.Mappings;
using ProjectManager.Application.Services;

namespace ProjectManager.Application.Extensions;

    /// <summary>
    /// Добавляет основные сервисы приложения в контейнер зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации зависимостей.</param>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавляет основные сервисы приложения в контейнер зависимостей.
        /// </summary>
        /// <param name="services">Коллекция сервисов для регистрации зависимостей.</param>
        public static void AddCoreApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddAutoMapper(typeof(TicketMappingProfile));
            services.AddAutoMapper(typeof(BoardMappingProfile));
            services.AddAutoMapper(typeof(ColumnMappingProfile));
            services.AddAutoMapper(typeof(RefreshTokenMappingProfile));

            services.AddScoped<IAuthentificationService, AuthentificationService>();
            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IColumnService, ColumnService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ITicketTransitionService, TicketTransitionService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserHelperService, UserHelperService>();

        }
    }
