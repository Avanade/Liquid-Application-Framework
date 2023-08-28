using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Messaging.Extensions.DependencyInjection;
using Liquid.Messaging.Interfaces;
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
        /// <see cref="IServiceCollectionLiquidExtension.AddScopedLiquidTelemetry{TInterface, TService}(IServiceCollection)"/>.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that will be consumed by this service instance.</typeparam>
        /// <param name="services">Extended service collection instance.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <param name="activateTelemetry">Indicates if telemetry interceptor must be registered.</param>
        public static IServiceCollection AddLiquidRabbitMqProducer<TEntity>(this IServiceCollection services, string sectionName, bool activateTelemetry = true)
        {
            services.TryAddTransient<IRabbitMqFactory, RabbitMqFactory>();

            if (activateTelemetry)
            {
                services.AddScoped((provider) =>
                {
                    var settings = provider.GetService<IConfiguration>().GetSection(sectionName).Get<RabbitMqProducerSettings>();
                    return ActivatorUtilities.CreateInstance<RabbitMqProducer<TEntity>>(provider, settings);
                });

                services.AddScopedLiquidTelemetry<ILiquidProducer<TEntity>, RabbitMqProducer<TEntity>>();
            }
            else
            {
                services.AddScoped<ILiquidProducer<TEntity>>((provider) =>
                {
                    var settings = provider.GetService<IConfiguration>().GetSection(sectionName).Get<RabbitMqProducerSettings>();
                    return ActivatorUtilities.CreateInstance<RabbitMqProducer<TEntity>>(provider, settings);
                });
            }

            return services;
        }

        /// <summary>
        /// Register Liquid resources for consumers 
        /// <see cref="IServiceCollectionExtensions.AddLiquidMessageConsumer{TService, TEntity}(IServiceCollection, Assembly[])"/>
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
        /// In order for consumers injected by this method to work correctly, you will need to register the Liquid settings
        /// <see cref="IServiceCollectionLiquidExtension.AddLiquidConfiguration(IServiceCollection)"/> and 
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

            if (activateTelemetry)
            {
                services.AddSingleton((provider) =>
                {
                    var settings = provider.GetService<IConfiguration>().GetSection(sectionName).Get<RabbitMqConsumerSettings>();
                    return ActivatorUtilities.CreateInstance<RabbitMqConsumer<TEntity>>(provider, settings);
                });


                services.AddSingletonLiquidTelemetry<ILiquidConsumer<TEntity>, RabbitMqConsumer<TEntity>>();
            }
            else
            {
                services.AddSingleton<ILiquidConsumer<TEntity>>((provider) =>
                {
                    var settings = provider.GetService<IConfiguration>().GetSection(sectionName).Get<RabbitMqConsumerSettings>();
                    return ActivatorUtilities.CreateInstance<RabbitMqConsumer<TEntity>>(provider, settings);
                });
            }


            return services;
        }
    }
}
