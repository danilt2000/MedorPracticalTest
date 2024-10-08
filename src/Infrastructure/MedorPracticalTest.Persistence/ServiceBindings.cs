using MedorPracticalTest.Persistence.Abstractions.Repositories;
using MedorPracticalTest.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MedorPracticalTest.Persistence
{
        public static class ServiceBindings
        {
                public static IServiceCollection AddApplicationPersistence(this IServiceCollection services)
                {
                        services.AddSingleton<IBitcoinRepository, BitcoinRepository>();
                        
                        return services;
                }
        }
}
