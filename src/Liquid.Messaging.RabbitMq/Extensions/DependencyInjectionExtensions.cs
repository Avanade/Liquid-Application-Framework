using AutoMapper;
using Liquid.Core.Interfaces;
using Liquid.Messaging.RabbitMq.Configuration;
using Liquid.Messaging.RabbitMq.Parameters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Liquid.Messaging.RabbitMq.Extensions
{
    /// <summary>
    /// Aws Service Collection Extensions.
    /// </summary>
    public static class DependencyInjectionExtensions
    {

        /// <summary>
        /// Adds the RabbitMq producer to service collection.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="advancedSettings">The advanced settings.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMqProducer<TMessage>(this IServiceCollection services, string connectionId, string exchange, RabbitMqParameterSettings advancedSettings = null, bool compressMessage = false)
        {
            var parameters = new RabbitMqProducerParameter(connectionId, exchange, advancedSettings, compressMessage);

            services.AddSingleton<ILightProducer<TMessage>>((sp) =>
                    new RabbitMqProducer<TMessage>(sp.GetService<ILiquidContext>(),
                                                   sp.GetService<ILoggerFactory>(),
                                                   sp.GetService<ILiquidConfiguration<RabbitMqSettings>>(),
                                                   parameters));
            return services;
        }


        /// <summary>
        /// Adds the RabbitMq consumer to service collection.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="exchange">The exchange.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="advancedSettings">The advanced settings.</param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMqConsumer<TConsumer, TMessage>(this IServiceCollection services, string connectionId, string exchange, string queue, RabbitMqParameterSettings advancedSettings = null)
            where TConsumer : RabbitMqConsumer<TMessage>
        {
            var parameters = new RabbitMqConsumerParameter(connectionId, exchange, queue, advancedSettings);

            services.AddSingleton<ILightConsumer<TMessage>>((sp) =>
                    (TConsumer)Activator.CreateInstance(typeof(TConsumer),
                                                        sp,
                                                        sp.GetService<IMediator>(),
                                                        sp.GetService<IMapper>(),
                                                        sp.GetService<ILiquidContext>(),
                                                        sp.GetService<ILoggerFactory>(),
                                                        sp.GetService<ILiquidConfiguration<RabbitMqSettings>>(),
                                                        parameters)
            );

            return services;
        }
    }
}
