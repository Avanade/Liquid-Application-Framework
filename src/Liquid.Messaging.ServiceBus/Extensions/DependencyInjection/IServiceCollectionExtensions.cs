using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Messaging.Extensions.DependencyInjection;
using Liquid.Messaging.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;
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
        /// <param name="sectionName">Configuration section name.</param>
        public static IServiceCollection AddServiceBusProducer<TEntity>(this IServiceCollection services, string sectionName)
        {
            services.TryAddTransient<IServiceBusFactory, ServiceBusFactory>();

            services.AddScoped((provider) => {
                return ActivatorUtilities.CreateInstance<ServiceBusProducer<TEntity>>(provider, sectionName);
            });

            services.AddScopedLiquidTelemetry<ILiquidProducer<TEntity>, ServiceBusProducer<TEntity>>();

            return services;
        }

        /// <summary>
        /// Register Liquid resources for consumers 
        /// <see cref="Messaging.Extensions.DependencyInjection.IServiceCollectionExtensions.AddLiquidMessageConsumer{TService, TEntity}(IServiceCollection, Assembly[])"/>
        /// and a <see cref="ServiceBusConsumer{TEntity}"/> service with its dependency, with 
        /// <see cref="IServiceCollectionLiquidExtension.AddLiquidTelemetryInterceptor{TInterface, TService}(IServiceCollection)"/>.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that will be consumed by this service instance.</typeparam>
        /// <typeparam name="TWorker">Type of implementation from <see cref="ILiquidWorker{TEntity}"/></typeparam>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <param name="assemblies">Array of assemblies that contains domain handlers implementation.</param>
        public static IServiceCollection AddLiquidServiceBusConsumer<TWorker, TEntity>(this IServiceCollection services, string sectionName,  params Assembly[] assemblies)
             where TWorker : class, ILiquidWorker<TEntity>
        {
            services.AddLiquidMessageConsumer<TWorker, TEntity>(assemblies);            

            services.AddConsumer<TEntity>(sectionName);

            return services;
        }

        /// <summary>
        /// Register a <see cref="ServiceBusConsumer{TEntity}"/> service with its dependency, and with 
        /// <see cref="IServiceCollectionLiquidExtension.AddLiquidTelemetryInterceptor{TInterface, TService}(IServiceCollection)"/>.
        /// In order for consumers injected by this method to work correctly, you will need to register the Liquid settings
        /// <see cref="IServiceCollectionLiquidExtension.AddLiquidConfiguration(IServiceCollection)"/> and 
        /// domain handlers/services in your build configurator.
        /// </summary>
        /// <typeparam name="TWorker">Type of implementation from <see cref="ILiquidWorker{TEntity}"/></typeparam>
        /// <typeparam name="TEntity">Type of entity that will be consumed by the service instance.</typeparam>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="sectionName">Configuration section name.</param>
        public static IServiceCollection AddLiquidServiceBusConsumer<TWorker, TEntity>(this IServiceCollection services, string sectionName)
            where TWorker : class, ILiquidWorker<TEntity>
        {
            services.AddLiquidWorkerService<TWorker, TEntity>();

            services.AddConsumer<TEntity>(sectionName);

            return services;
        }

        private static IServiceCollection AddConsumer<TEntity>(this IServiceCollection services, string sectionName)
        {
            services.AddTransient<IServiceBusFactory, ServiceBusFactory>();

            services.AddSingleton((provider) => {
                return ActivatorUtilities.CreateInstance<ServiceBusConsumer<TEntity>>(provider, sectionName);
            });

            services.AddSingletonLiquidTelemetry<ILiquidConsumer<TEntity>, ServiceBusConsumer<TEntity>>();

            return services;
        }
    }
}
