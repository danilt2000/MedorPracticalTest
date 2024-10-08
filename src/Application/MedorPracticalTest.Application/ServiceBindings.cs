using FluentValidation;
using MediatR;
using MedorPracticalTest.Application.Extensions;
using MedorPracticalTest.BitcoinPriceService;
using MedorPracticalTest.CnbExchangeService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MedorPracticalTest.Persistence;

namespace MedorPracticalTest.Application
{
        public static class ServiceBindings
        {
                public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
                {
                        services.AddBitcoinPriceServices();

                        services.AddCnbExchangeServices();
                        //services.AddMediatR(typeof(GetCurrentBitcoinPriceRequestHandler).Assembly);

                        services.AddMediatR(config =>
                        {
                                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
                        });

                        services.AddApplicationPersistence(configuration);
                        //services.AddMediatR(config =>
                        //{
                        //        config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                        //        config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
                        //});

                        //services.AddMediatR(typeof(GetCurrentBitcoinPriceRequestHandler).Assembly);

                        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

                        return services;
                }
        }
}
