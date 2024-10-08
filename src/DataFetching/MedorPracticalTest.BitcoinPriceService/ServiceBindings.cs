using MedorPracticalTest.BitcoinPriceService.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedorPracticalTest.BitcoinPriceService
{
        public static class ServiceBindings
        {
                public static IServiceCollection AddBitcoinPriceServices(this IServiceCollection services)
                {
                        services.AddScoped<IBitcoinPriceService, Services.BitcoinPriceService>();

                        return services;
                }
        }
}
