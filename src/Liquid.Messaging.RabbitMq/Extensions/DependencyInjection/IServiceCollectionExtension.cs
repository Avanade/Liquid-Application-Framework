using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Interfaces;
using Liquid.Messaging.RabbitMq.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Liquid.Messaging.RabbitMq.Extensions.DependencyInjection
{
    /// <summary>
    /// Startup extension methods. Used to configure the startup application.
    /// </summary>
    public static class IServiceCollectionExtension
    {

        /// <summary>
        /// Register a <see cref="RabbitMqConsumer{TEntity}"/> with its dependency, and with 
        /// <see cref="IServiceCollectionLiquidExtension.AddSingletonLiquidTelemetry{TInterface, TService}(IServiceCollection)"/>.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that will be consumed by this service instance.</typeparam>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <param name="activateTelemetry">Indicates if telemetry interceptor must be registered.</param>
        public static IServiceCollection AddLiquidRabbitMqProducer<TEntity>(this IServiceCollection services, string sectionName, bool activateTelemetry = true)
        {
            services.TryAddTransient<IRabbitMqFactory, RabbitMqFactory>();

            services.AddOptions<RabbitMqProducerSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(sectionName).Bind(settings);
            });

            if (activateTelemetry)
            {
                services.AddSingleton<RabbitMqProducer<TEntity>>();
                services.AddSingletonLiquidTelemetry<ILiquidProducer<TEntity>, RabbitMqProducer<TEntity>>();
            }
            else
            {
                services.AddSingleton<ILiquidProducer<TEntity>, RabbitMqProducer<TEntity>>();
            }

            return services;
        }

        /// <summary>
        /// Register Liquid resources for consumers 
        /// <see cref="IServiceCollectionCoreExtensions.AddLiquidMessageConsumer{TService, TEntity}(IServiceCollection, Assembly[])"/>
        /// and a <see cref="RabbitMqConsumer{TEntity}"/> service with its dependency, with 
        /// <see cref="IServiceCollectionLiquidExtension.AddLiquidTelemetryInterceptor{TInterface, TService}(IServiceCollection)"/>.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that will be consumed by this service instance.</typeparam>
        /// <typeparam name="TWorker">Type of implementation from <see cref="ILiquidWorker{TEntity}"/></typeparam>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <param name="activateTelemetry">Indicates if telemetry interceptor must be registered.</param>
        /// <param name="assemblies">Array of assemblies that contains domain handlers implementation.</param>
        public static IServiceCollection AddLiquidRabbitMqConsumer<TWorker, TEntity>(this IServiceCollection services, string sectionName, bool activateTelemetry = true, params Assembly[] assemblies)
             where TWorker : class, ILiquidWorker<TEntity>
        {
            services.AddLiquidMessageConsumer<TWorker, TEntity>(assemblies);

            services.AddConsumer<TEntity>(sectionName, activateTelemetry);

            return services;
        }

        /// <summary>
        /// Register a <see cref="RabbitMqConsumer{TEntity}"/> service with its dependency, and with 
        /// <see cref="IServiceCollectionLiquidExtension.AddLiquidTelemetryInterceptor{TInterface, TService}(IServiceCollection)"/>.
        /// In order for consumers injected by this method to work correctly, you will need to register the Liquid settings and 
        /// domain handlers/services in your build configurator.
        /// </summary>
        /// <typeparam name="TWorker">Type of implementation from <see cref="ILiquidWorker{TEntity}"/></typeparam>
        /// <typeparam name="TEntity">Type of entity that will be consumed by the service instance.</typeparam>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <param name="activateTelemetry">Indicates if telemetry interceptor must be registered.</param>
        public static IServiceCollection AddLiquidRabbitMqConsumer<TWorker, TEntity>(this IServiceCollection services, string sectionName, bool activateTelemetry = true)
            where TWorker : class, ILiquidWorker<TEntity>
        {
            services.AddLiquidWorkerService<TWorker, TEntity>();

            services.AddConsumer<TEntity>(sectionName, activateTelemetry);

            return services;
        }

        private static IServiceCollection AddConsumer<TEntity>(this IServiceCollection services, string sectionName, bool activateTelemetry = true)
        {
            services.AddSingleton<IRabbitMqFactory, RabbitMqFactory>();

            services.AddOptions<RabbitMqConsumerSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(sectionName).Bind(settings);
            });

            if (activateTelemetry)
            {
                services.AddSingleton<RabbitMqConsumer<TEntity>>();

                services.AddSingletonLiquidTelemetry<ILiquidConsumer<TEntity>, RabbitMqConsumer<TEntity>>();
            }
            else
            {
                services.AddSingleton<ILiquidConsumer<TEntity>, RabbitMqConsumer<TEntity>>();
            }


            return services;
        }
    }
}
