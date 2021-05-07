using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Kafka.Configuration;
using Liquid.Messaging.Kafka.Parameters;
using Liquid.Messaging.Kafka.Tests.Messages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Kafka.Tests.Consumers
{
    /// <summary>
    /// Kafka Test Consumer Class.
    /// </summary>
    /// <seealso cref="KafkaConsumer{KafkaTestMessage}" />
    public class KafkaTestConsumer : KafkaConsumer<KafkaTestMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaTestConsumer"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="kafkaConsumerParameter">The kafka consumer parameter.</param>
        public KafkaTestConsumer(IServiceProvider serviceProvider, 
                                 IMediator mediator, 
                                 IMapper mapper, 
                                 ILightContextFactory contextFactory, 
                                 ILightTelemetryFactory telemetryFactory, 
                                 ILoggerFactory loggerFactory, 
                                 ILightMessagingConfiguration<KafkaSettings> messagingConfiguration, 
                                 KafkaConsumerParameter kafkaConsumerParameter) : base(serviceProvider, mediator, mapper, contextFactory, telemetryFactory, loggerFactory, messagingConfiguration, kafkaConsumerParameter)
        {
        }

        /// <summary>
        /// Consumes the message from  subscription asynchronous.
        /// </summary>
        /// <param name="message">The message to be consumed.</param>
        /// <param name="headers">The custom headers of message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<bool> ConsumeAsync(KafkaTestMessage message, IDictionary<string, object> headers, CancellationToken cancellationToken)
        {
            KafkaTestMessage.Self = message;
            return await Task.FromResult(true);
        }
    }
}