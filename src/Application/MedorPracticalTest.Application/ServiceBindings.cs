using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using MedorPracticalTest.Application.Extensions;
using MedorPracticalTest.BitcoinPriceService;
using MedorPracticalTest.Application.Requests.Bitcoins.Queries.GetCurrentBitcoinPriceRequest;
using MedorPracticalTest.CnbExchangeService;

namespace MedorPracticalTest.Application
{
        public static class ServiceBindings
        {
                public static IServiceCollection AddApplicationServices(this IServiceCollection services)
                {
                        services.AddBitcoinPriceServices();

                        services.AddCnbExchangeServices();
                        //services.AddMediatR(typeof(GetCurrentBitcoinPriceRequestHandler).Assembly);

                        services.AddMediatR(config =>
                        {
                                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
                        });

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
