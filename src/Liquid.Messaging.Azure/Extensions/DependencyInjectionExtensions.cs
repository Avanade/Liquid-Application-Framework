using AutoMapper;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Azure.Parameters;
using Liquid.Messaging.Azure.Configuration;
using Liquid.Messaging.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Liquid.Messaging.Azure.Extensions
{
    /// <summary>
    /// Aws Service Collection Extensions.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds the service bus producer to service collection.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBusProducer<TMessage>(this IServiceCollection services, string connectionId, string topic, bool compressMessage = false)
        {
            var parameters = new ServiceBusProducerParameter(connectionId, topic, compressMessage);

            services.AddSingleton<ILightMessagingConfiguration<ServiceBusSettings>, ServiceBusConfiguration>();
            services.AddSingleton<ILightProducer<TMessage>>((sp) =>
                    new ServiceBusProducer<TMessage>(sp.GetService<ILightContextFactory>(),
                                                     sp.GetService<ILightTelemetryFactory>(),
                                                     sp.GetService<ILoggerFactory>(),
                                                     sp.GetService<ILightMessagingConfiguration<ServiceBusSettings>>(),
                                                     parameters));
            return services;
        }

        /// <summary>
        /// Adds the service bus consumer to service collection.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="autoComplete">if set to <c>true</c> [automatic complete].</param>
        /// <param name="maxConcurrentCalls">The maximum concurrent calls.</param>
        /// <returns></returns>
        public static IServiceCollection AddServiceBusConsumer<TConsumer, TMessage>(this IServiceCollection services, string connectionId, string topic, string subscription, bool autoComplete = false, int maxConcurrentCalls = 1) 
            where TConsumer : ServiceBusConsumer<TMessage>
        {
            var parameters = new ServiceBusConsumerParameter(connectionId, topic, subscription, autoComplete, maxConcurrentCalls);

            services.AddSingleton<ILightMessagingConfiguration<ServiceBusSettings>, ServiceBusConfiguration>();
            services.AddSingleton<ILightConsumer<TMessage>>((sp) =>
                    (TConsumer)Activator.CreateInstance(typeof(TConsumer),
                                                        sp,
                                                        sp.GetService<IMediator>(),
                                                        sp.GetService<IMapper>(),
                                                        sp.GetService<ILightContextFactory>(),
                                                        sp.GetService<ILightTelemetryFactory>(),
                                                        sp.GetService<ILoggerFactory>(),
                                                        sp.GetService<ILightMessagingConfiguration<ServiceBusSettings>>(),
                                                        parameters)
            );

            return services;
        }
    }
}
