using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Liquid.Domain.Extensions.DependencyInjection;
using Liquid.Messaging.Decorators;
using Liquid.Messaging.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Liquid.Messaging.Extensions.DependencyInjection
{
    /// <summary>
    /// Startup extension methods. Used to configure the startup application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Register liquid settings <see cref="IServiceCollectionLiquidExtension.AddLiquidConfiguration(IServiceCollection)"/>,
        /// messaging pipeline with Liquid Decorators <see cref="AddLiquidPipeline(IServiceCollection)"/>, and domain handlers
        /// <see cref="Domain.Extensions.DependencyInjection.IServiceCollectionExtensions.AddLiquidHandlers(IServiceCollection, bool, bool, Assembly[])"/>
        /// that exists in <paramref name="assemblies"/>, with telemetry and validators.
        /// </summary>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="assemblies">Array of assemblies that contains domain handlers implementation.</param>
        /// <returns></returns>
        public static IServiceCollection AddLiquidForConsumer(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddLiquidConfiguration();
            services.AddLiquidPipeline();
            services.AddAutoMapper(assemblies);
            services.AddLiquidHandlers(withTelemetry: true, withValidators: true, assemblies);

            return services;
        }
        /// <summary>
        /// Register <see cref="LiquidPipeline"/> and aditional behaviors
        /// <see cref="LiquidContextDecorator"/>, <see cref="LiquidScopedLoggingDecorator"/>
        /// and <see cref="LiquidCultureDecorator"/>. These additional behaviors will be 
        /// performed in the reverse order they were recgistered.
        /// </summary>
        /// <param name="services">Extended service collection instance.</param>
        public static IServiceCollection AddLiquidPipeline(this IServiceCollection services)
        {
            services.AddScoped<LiquidContext>();
            services.AddTransient<ILiquidPipeline, LiquidPipeline>();
            services.Decorate<ILiquidPipeline, LiquidContextDecorator>();
            services.Decorate<ILiquidPipeline, LiquidScopedLoggingDecorator>();
            services.Decorate<ILiquidPipeline, LiquidCultureDecorator>();

            return services;
        }
        /// <summary>
        /// Changes the previously registered <typeparamref name="TInterface"/> service descriptor 
        /// to the <typeparamref name="TDecorator"/> service.
        /// </summary>
        /// <typeparam name="TInterface">Interface type of service that should be decorated.</typeparam>
        /// <typeparam name="TDecorator">Type of decorator service implementation.</typeparam>
        /// <param name="services">Extended service collection instance.</param>
        public static IServiceCollection Decorate<TInterface, TDecorator>(this IServiceCollection services)
           where TInterface : class
           where TDecorator : class, TInterface
        {
            ServiceDescriptor innerDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(TInterface));

            if (innerDescriptor == null) { throw new InvalidOperationException($"{typeof(TInterface).Name} is not registered"); }

            var objectFactory = ActivatorUtilities.CreateFactory(
              typeof(TDecorator),
              new[] { typeof(TInterface) });

            services.Replace(ServiceDescriptor.Describe(
              typeof(TInterface),
              s => (TInterface)objectFactory(s, new[] { s.CreateInstance(innerDescriptor) }), innerDescriptor.Lifetime)
            );

            return services;
        }

        private static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
                return descriptor.ImplementationInstance;

            if (descriptor.ImplementationFactory != null)
                return descriptor.ImplementationFactory(services);

            return ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType);
        }

    }
}
