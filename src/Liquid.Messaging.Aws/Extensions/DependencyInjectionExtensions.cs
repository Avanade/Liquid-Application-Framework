using AutoMapper;
using Liquid.Core.Interfaces;
using Liquid.Messaging.Aws.Configuration;
using Liquid.Messaging.Aws.Parameters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Liquid.Messaging.Aws.Extensions
{
    /// <summary>
    /// Aws Service Collection Extensions.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds a SNS producer to service collection.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="messageStructure">The message structure.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        /// <returns></returns>
        public static IServiceCollection AddSnsProducer<TMessage>(this IServiceCollection services, string connectionId, string topic, string messageStructure = "json", bool compressMessage = false)
        {
            var parameters = new SnsProducerParameter(connectionId, topic, messageStructure, compressMessage);

            services.AddSingleton<ILightProducer<TMessage>>((sp) =>
                    new SnsProducer<TMessage>(sp.GetService<ILiquidContext>(),
                                              sp.GetService<ILoggerFactory>(),
                                              sp.GetService<ILiquidConfiguration<AwsMessagingSettings>>(),
                                              parameters));

            return services;
        }

        /// <summary>
        /// Adds a Sqs producer to service collection.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="compressMessage">if set to <c>true</c> [compress message].</param>
        /// <returns></returns>
        public static IServiceCollection AddSqsProducer<TMessage>(this IServiceCollection services, string connectionId, string queue, bool compressMessage = false)
        {
            var parameters = new SqsProducerParameter(connectionId, queue, compressMessage);

            services.AddSingleton<ILightProducer<TMessage>>((sp) =>
                    new SqsProducer<TMessage>(sp.GetService<ILiquidContext>(),
                                              sp.GetService<ILoggerFactory>(),
                                              sp.GetService<ILiquidConfiguration<AwsMessagingSettings>>(),
                                              parameters));

            return services;
        }

        /// <summary>
        /// Adds the SQS consumer to service collection.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="autoComplete">if set to <c>true</c> [automatic complete].</param>
        /// <returns></returns>
        public static IServiceCollection AddSqsConsumer<TConsumer, TMessage>(this IServiceCollection services, string connectionId, string queue, bool autoComplete = false)
            where TConsumer : SqsConsumer<TMessage>
        {
            var parameters = new SqsConsumerParameter(connectionId, queue, autoComplete);

            services.AddSingleton<ILightConsumer<TMessage>>((sp) =>
                    (TConsumer)Activator.CreateInstance(typeof(TConsumer),
                                                        sp,
                                                        sp.GetService<IMediator>(),
                                                        sp.GetService<IMapper>(),
                                                        sp.GetService<ILiquidContext>(),
                                                        sp.GetService<ILoggerFactory>(),
                                                        sp.GetService<ILiquidConfiguration<AwsMessagingSettings>>(),
                                                        parameters)
            );

            return services;
        }
    }
}
