using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Messaging.Extensions.DependencyInjection;
using Liquid.Messaging.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Liquid.Messaging.ServiceBus.Extensions.DependencyInjection
{
    /// <summary>
    /// Startup extension methods. Used to configure the startup application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Register a <see cref="ServiceBusConsumer{TEntity}"/> with its dependency, and with 
        /// <see cref="IServiceCollectionLiquidExtension.AddLiquidTelemetryInterceptor{TInterface, TService}(IServiceCollection)"/>.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that will be consumed by this service instance.</typeparam>
        /// <param name="services">Extended service collection instance.</param>
        public static IServiceCollection AddServiceBusProducer<TEntity>(this IServiceCollection services)
        {
            if (services.FirstOrDefault((ServiceDescriptor x) => x.ServiceType == typeof(IServiceBusFactory)) == null)
            {
                services.AddTransient<IServiceBusFactory, ServiceBusFactory>();
            }

            services.AddScoped<ServiceBusProducer<TEntity>>();

            services.AddLiquidTelemetryInterceptor<ILiquidProducer<TEntity>, ServiceBusProducer<TEntity>>();

            return services;
        }

        /// <summary>
        /// Register Liquid resources for consumers 
        /// <see cref="Messaging.Extensions.DependencyInjection.IServiceCollectionExtensions.AddLiquidForConsumer(IServiceCollection, Assembly[])"/>
        /// and a <see cref="ServiceBusConsumer{TEntity}"/> service with its dependency, with 
        /// <see cref="IServiceCollectionLiquidExtension.AddLiquidTelemetryInterceptor{TInterface, TService}(IServiceCollection)"/>.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that will be consumed by this service instance.</typeparam>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="assemblies">Array of assemblies that contains domain handlers implementation.</param>
        public static IServiceCollection AddLiquidServiceBusConsumer<TEntity>(this IServiceCollection services, params Assembly[] assemblies)
        {            
            services.AddLiquidForConsumer<TEntity>(assemblies);

            if (services.FirstOrDefault((ServiceDescriptor x) => x.ServiceType == typeof(IServiceBusFactory)) == null)
            {
                services.AddTransient<IServiceBusFactory, ServiceBusFactory>();
            }

            services.AddSingleton<ServiceBusConsumer<TEntity>>();

            services.AddLiquidTelemetryInterceptor<ILiquidConsumer<TEntity>, ServiceBusConsumer<TEntity>>();     

            return services;
        }

        /// <summary>
        /// Register a <see cref="ServiceBusConsumer{TEntity}"/> service with its dependency, and with 
        /// <see cref="IServiceCollectionLiquidExtension.AddLiquidTelemetryInterceptor{TInterface, TService}(IServiceCollection)"/>.
        /// In order for consumers injected by this method to work correctly, you will need to register the Liquid settings
        /// <see cref="IServiceCollectionLiquidExtension.AddLiquidConfiguration(IServiceCollection)"/> and 
        /// domain handlers/services in your build configurator.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that will be consumed by the service instance.</typeparam>
        /// <param name="services">Extended service collection instance.</param>
        public static IServiceCollection AddLiquidServiceBusConsumer<TEntity>(this IServiceCollection services)
        {
            if (services.FirstOrDefault((ServiceDescriptor x) => x.ServiceType == typeof(IServiceBusFactory)) == null)
            {
                services.AddTransient<IServiceBusFactory, ServiceBusFactory>();
            }

            services.AddSingleton<ServiceBusConsumer<TEntity>>();

            services.AddLiquidTelemetryInterceptor<ILiquidConsumer<TEntity>, ServiceBusConsumer<TEntity>>();

            return services;
        }
    }
}
