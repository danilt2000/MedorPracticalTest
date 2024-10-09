using MedorPracticalTest.WebAPI.Extensions;
using MedorPracticalTest.WebAPI.Middlewares;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

namespace MedorPracticalTest.WebAPI;

internal class Program
{
        public static void Main(string[] args)//Todo introduce startup 
        {
                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day));

                builder.Services.AddServices(builder.Configuration);

#if RELEASE//Todo make it more nicer 
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ListenAnyIP(80);
        });
#endif
                builder.Services.AddApiVersioningConfiguration();

                builder.Services.AddControllers();

                builder.Services.AddEndpointsApiExplorer();

                var app = builder.Build();

                var versionDescProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                app.AddSwagger();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                        foreach (var desc in versionDescProvider.ApiVersionDescriptions)
                        {
                                options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", $"MedorPracticalTest - {desc.GroupName.ToUpper()}");
                        }
                });

                app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                app.Run();
        }
}