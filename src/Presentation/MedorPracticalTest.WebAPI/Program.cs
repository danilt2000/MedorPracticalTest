using MedorPracticalTest.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
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

                builder.Services.AddControllers();

                builder.Services.AddServices(builder.Configuration);

                builder.Services.AddApiVersioning(options =>
                {
                        options.AssumeDefaultVersionWhenUnspecified = true;
                        options.DefaultApiVersion = new ApiVersion(1, 0);
                        options.ReportApiVersions = true;
                });

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                app.Run();
        }
}