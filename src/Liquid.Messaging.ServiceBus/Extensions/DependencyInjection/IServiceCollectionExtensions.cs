using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Interfaces;
using Liquid.Messaging.ServiceBus.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Configuration;

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
        /// <param name="entityPath">Entity path to configure this producer.</param>
        /// <param name="activateTelemetry">Indicates if telemetry interceptor must be registered.</param>
        public static IServiceCollection AddLiquidServiceBusProducer<TEntity>(this IServiceCollection services, 
            string sectionName, string entityPath, bool activateTelemetry = true)
        {
            services.AddOptions<ServiceBusSettings>()
             .Configure<IConfiguration>((settings, configuration) =>
             {
                 configuration.GetSection(sectionName).Bind(settings);
             });

            services.TryAddTransient<IServiceBusFactory, ServiceBusFactory>();

            if (activateTelemetry)
            {
                services.AddScoped((provider) =>
                {
                    return ActivatorUtilities.CreateInstance<ServiceBusProducer<TEntity>>(provider, entityPath);
                });

                services.AddScopedLiquidTelemetry<ILiquidProducer<TEntity>, ServiceBusProducer<TEntity>>();
            }
            else
            {
                services.AddScoped<ILiquidProducer<TEntity>>((provider) =>
                {
                    return ActivatorUtilities.CreateInstance<ServiceBusProducer<TEntity>>(provider, entityPath);
                });
            }

            return services;
        }

        /// <summary>
        /// Register Liquid resources for consumers 
        /// <see cref="IServiceCollectionCoreExtensions.AddLiquidMessageConsumer{TService, TEntity}(IServiceCollection, Assembly[])"/>
        /// and a <see cref="ServiceBusConsumer{TEntity}"/> service with its dependency, with 
        /// <see cref="IServiceCollectionLiquidExtension.AddLiquidTelemetryInterceptor{TInterface, TService}(IServiceCollection)"/>.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that will be consumed by this service instance.</typeparam>
        /// <typeparam name="TWorker">Type of implementation from <see cref="ILiquidWorker{TEntity}"/></typeparam>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <param name="entityPath">Entity name to configure for this worker.</param>
        /// <param name="activateTelemetry">Indicates if telemetry interceptor must be registered.</param>
        /// <param name="assemblies">Array of assemblies that contains domain handlers implementation.</param>
        public static IServiceCollection AddLiquidServiceBusConsumer<TWorker, TEntity>(this IServiceCollection services
            , string sectionName
            , string entityPath
            , bool activateTelemetry = true
            , params Assembly[] assemblies)
             where TWorker : class, ILiquidWorker<TEntity>
        {
            services.AddOptions<ServiceBusSettings>()
             .Configure<IConfiguration>((settings, configuration) =>
             {
                 configuration.GetSection(sectionName).Bind(settings);
             });
            services.AddLiquidMessageConsumer<TWorker, TEntity>(assemblies);

            services.AddConsumer<TEntity>(entityPath, activateTelemetry);

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
        /// <param name="activateTelemetry">Indicates if telemetry interceptor must be registered.</param>
        public static IServiceCollection AddLiquidServiceBusConsumer<TWorker, TEntity>(this IServiceCollection services, string sectionName, bool activateTelemetry = true)
            where TWorker : class, ILiquidWorker<TEntity>
        {
            services.AddLiquidWorkerService<TWorker, TEntity>();

            services.AddConsumer<TEntity>(sectionName, activateTelemetry);

            return services;
        }

        private static IServiceCollection AddConsumer<TEntity>(this IServiceCollection services, string sectionName, bool activateTelemetry = true)
        {
            services.AddTransient<IServiceBusFactory, ServiceBusFactory>();

            if (activateTelemetry)
            {
                services.AddSingleton((provider) =>
                {
                    return ActivatorUtilities.CreateInstance<ServiceBusConsumer<TEntity>>(provider, sectionName);
                });

                services.AddSingletonLiquidTelemetry<ILiquidConsumer<TEntity>, ServiceBusConsumer<TEntity>>();
            }
            else
            {
                services.AddSingleton<ILiquidConsumer<TEntity>>((provider) =>
                {
                    return ActivatorUtilities.CreateInstance<ServiceBusConsumer<TEntity>>(provider, sectionName);
                });
            }

            return services;
        }
    }
}
