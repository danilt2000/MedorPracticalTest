using MedorPracticalTest.WebAPI.Extensions;
using MedorPracticalTest.WebAPI.Middlewares;
using Serilog;

namespace MedorPracticalTest.WebAPI;

internal class Program
{
        public static void Main(string[] args)
        {
                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day));

                builder.Services.AddServices(builder.Configuration);
                
                builder.Services.AddApiVersioningConfiguration();
                
                builder.Services.AddControllers();

                builder.Services.AddEndpointsApiExplorer();

                var app = builder.Build();

                app.AddSwagger();
                
                app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                app.Run();
        }
}