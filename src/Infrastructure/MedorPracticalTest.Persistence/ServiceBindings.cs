using MedorPracticalTest.Persistence.Abstractions.Repositories;
using MedorPracticalTest.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedorPracticalTest.Persistence
{
        public static class ServiceBindings
        {
                public static IServiceCollection AddApplicationPersistence(this IServiceCollection services, IConfiguration configuration)
                {
                        services.AddTransient<IBitcoinRepository, BitcoinRepository>(provider =>
                                new BitcoinRepository(configuration["DbConnectionString"]!));

                        return services;
                }
        }
}
