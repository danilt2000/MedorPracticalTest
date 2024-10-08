using MedorPracticalTest.CnbExchangeService.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedorPracticalTest.CnbExchangeService
{
        public static class ServiceBindings
        {
                public static IServiceCollection AddCnbExchangeServices(this IServiceCollection services)
                {
                        services.AddScoped<ICnbExchangeService, Services.CnbExchangeService>();

                        return services;
                }
        }
}
