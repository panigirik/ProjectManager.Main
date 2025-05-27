using ProjectManager.Application.Extensions;
using ProjectManager.ExternalServices.Extensions;
using ProjectManager.Identity.Extensions;
using ProjectManager.Main.ExceptionsHandling;
using ProjectManager.Persistence.Extensions;
using ProjectManager.ValidationServices.Extensions;
using Serilog;
using Prometheus;
    
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
            builder.WebHost.UseUrls("http://0.0.0.0:5258");
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

            app.UseRouting();

            app.UseHttpMetrics(); // <-- после Routing, до Endpoints
            app.UseCors("AllowAllOrigins");
            app.UseAuthorization();
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics(); // /metrics
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    options.RoutePrefix = string.Empty;
                });
            }
            Metrics.SuppressDefaultMetrics(); 
            app.Run();

        }
    }
}