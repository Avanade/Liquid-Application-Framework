using AutoMapper;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Kafka.Parameters;
using Liquid.Messaging.Kafka.Configuration;
using Liquid.Messaging.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Liquid.Messaging.Kafka.Extensions
{
    /// <summary>
    /// Aws Service Collection Extensions.
    /// </summary>
    public static class DependencyInjectionExtensions
    {

        /// <summary>
        /// Adds the kafka producer to service collection.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        /// <returns></returns>
        public static IServiceCollection AddKafkaProducer<TMessage>(this IServiceCollection services, string connectionId, string topic, bool compressMessage = false)
        {
            var parameters = new KafkaProducerParameter(connectionId, topic, compressMessage);

            services.AddSingleton<ILightMessagingConfiguration<KafkaSettings>, KafkaConfiguration>();
            services.AddSingleton<ILightProducer<TMessage>>((sp) =>
                    new KafkaProducer<TMessage>(sp.GetService<ILightContextFactory>(),
                                                 sp.GetService<ILightTelemetryFactory>(),
                                                 sp.GetService<ILoggerFactory>(),
                                                 sp.GetService<ILightMessagingConfiguration<KafkaSettings>>(),
                                                 parameters));
            return services;
        }


        /// <summary>
        /// Adds the kafka consumer to service collection.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="autoComplete">if set to <c>true</c> [automatic complete].</param>
        /// <returns></returns>
        public static IServiceCollection AddKafkaConsumer<TConsumer, TMessage>(this IServiceCollection services, string connectionId, string topic, bool autoComplete = false)
            where TConsumer : KafkaConsumer<TMessage>
        {
            var parameters = new KafkaConsumerParameter(connectionId, topic, autoComplete);

            services.AddSingleton<ILightMessagingConfiguration<KafkaSettings>, KafkaConfiguration>();
            services.AddSingleton<ILightConsumer<TMessage>>((sp) =>
                    (TConsumer)Activator.CreateInstance(typeof(TConsumer),
                                                        sp,
                                                        sp.GetService<IMediator>(),
                                                        sp.GetService<IMapper>(),
                                                        sp.GetService<ILightContextFactory>(),
                                                        sp.GetService<ILightTelemetryFactory>(),
                                                        sp.GetService<ILoggerFactory>(),
                                                        sp.GetService<ILightMessagingConfiguration<KafkaSettings>>(),
                                                        parameters)
            );

            return services;
        }
    }
}
