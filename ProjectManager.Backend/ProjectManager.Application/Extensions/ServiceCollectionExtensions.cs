using Microsoft.Extensions.DependencyInjection;
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

            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IColumnService, ColumnService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IUserService, UserService>();

        }
    }
