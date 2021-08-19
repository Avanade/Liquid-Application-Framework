using AutoMapper;
using Liquid.Core.Interfaces;
using Liquid.Messaging.Gcp.Configuration;
using Liquid.Messaging.Gcp.Factories;
using Liquid.Messaging.Gcp.Parameters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Liquid.Messaging.Gcp.Extensions
{
    /// <summary>
    /// Aws Service Collection Extensions.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds the pub sub producer to service collection.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        /// <param name="emulated">if set to <c>true</c> [emulated].</param>
        /// <returns></returns>
        public static IServiceCollection AddPubSubProducer<TMessage>(this IServiceCollection services, string connectionId, string topic, bool compressMessage = false, bool emulated = false)
        {
            var parameters = new PubSubProducerParameter(connectionId, topic, compressMessage);

            services.AddSingleton<IPubSubClientFactory>((sp) =>
            {
                if (emulated) return new EmulatedPubSubClientFactory();
                else return new DefaultPubSubClientFactory();
            });
            services.AddSingleton<ILightProducer<TMessage>>((sp) =>
                    new PubSubProducer<TMessage>(sp.GetService<ILiquidContext>(),
                                                 sp.GetService<ILoggerFactory>(),
                                                 sp.GetService<IPubSubClientFactory>(),
                                                 sp.GetService<ILiquidConfiguration<PubSubSettings>>(),
                                                 parameters));
            return services;
        }

        /// <summary>
        /// Adds the pub sub consumer to service collection.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="autoComplete">if set to <c>true</c> [automatic complete].</param>
        /// <param name="emulated">if set to <c>true</c> [emulated].</param>
        /// <returns></returns>
        public static IServiceCollection AddPubSubConsumer<TConsumer, TMessage>(this IServiceCollection services, string connectionId, string topic, string subscription, bool autoComplete = false, bool emulated = false)
            where TConsumer : PubSubConsumer<TMessage>
        {
            var parameters = new PubSubConsumerParameter(connectionId, topic, subscription, autoComplete);

            services.AddSingleton<IPubSubClientFactory>((sp) =>
            {
                if (emulated) return new EmulatedPubSubClientFactory();
                else return new DefaultPubSubClientFactory();
            });
            services.AddSingleton<ILightConsumer<TMessage>>((sp) =>
                    (TConsumer)Activator.CreateInstance(typeof(TConsumer),
                                                        sp,
                                                        sp.GetService<IMediator>(),
                                                        sp.GetService<IMapper>(),
                                                        sp.GetService<ILiquidContext>(),
                                                        sp.GetService<ILoggerFactory>(),
                                                        sp.GetService<IPubSubClientFactory>(),
                                                        sp.GetService<ILiquidConfiguration<PubSubSettings>>(),
                                                        parameters)
            );

            return services;
        }
    }
}
