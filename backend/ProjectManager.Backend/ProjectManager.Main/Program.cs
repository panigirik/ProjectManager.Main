using ProjectManager.Application.Extensions;
using ProjectManager.ExternalServices.Extensions;
using ProjectManager.Identity.Extensions;
using ProjectManager.Main.ExceptionsHandling;
using ProjectManager.Persistence.Extensions;
using ProjectManager.ValidationServices.Extensions;
using Serilog;

namespace ProjectManager.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCoreApplicationServices();
            builder.Services.AddControllersWithViews();
            builder.Services.AddInfrastructureIdentityServices(builder.Configuration);
            builder.Services.AddInfrastructureValidationServices();
            builder.Services.AddInfrastructureRepositoriesServices();
            builder.Services.AddInfrastructureExternalServices();
            builder.Services.AddSwaggerAuthentication();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            
            var app = builder.Build();
            app.UseSerilogRequestLogging();
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
            app.UseCors("AllowAllOrigins");
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseRouting();
            app.UseAuthorization();

            app.Run();
        }
    }
}