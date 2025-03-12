using ProjectManager.Application.Extensions;
using ProjectManager.Identity.Extensions;
using ProjectManager.Persistence.Extensions;

namespace ProjectManager.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCoreApplicationServices();
            builder.Services.AddControllersWithViews();
            builder.Services.AddInfrastructureIdentityServices(builder.Configuration);
            builder.Services.AddInfrastructureRepositoriesServices();
            builder.Services.AddSwaggerAuthentication();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    options.RoutePrefix = string.Empty;
                });
            }

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.MapControllers();
            app.UseRouting();
            app.UseAuthorization();

            app.Run();
        }
    }
}