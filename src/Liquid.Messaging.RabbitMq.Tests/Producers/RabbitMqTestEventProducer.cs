using System.Collections.Generic;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.RabbitMq.Attributes;
using Liquid.Messaging.RabbitMq.Configuration;
using Liquid.Messaging.RabbitMq.Tests.Messages;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.RabbitMq.Tests.Producers
{
    /// <summary>
    /// RabbitMqTestEventProducer Class.
    /// </summary>
    /// <seealso cref="RabbitMqProducer{RabbitMqTestMessage}" />
    [RabbitMqProducer("TestRabbitMq", "TestMessageTopic")]
    public class RabbitMqTestEventProducer : RabbitMqProducer<RabbitMqTestMessage>
    {
        /// <summary>
        /// Gets the rabbit mq settings.
        /// </summary>
        /// <value>
        /// The rabbit mq settings.
        /// </value>
        public override RabbitMqSettings RabbitMqSettings => new RabbitMqSettings
        {
            ExchangeType = "topic",
            AutoAck = false,
            RequestHeartBeatInSeconds = 60,
            Expiration = "60000",
            Global = false,
            Persistent = false,
            PrefetchCount = 10,
            Prefetch = 0,
            AutoDelete = false,
            Durable = false,
            QueueArguments = null,
            ExchangeArguments = null
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqTestEventProducer"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        public RabbitMqTestEventProducer(ILightContextFactory contextFactory,
                                          ILightTelemetryFactory telemetryFactory,
                                          ILoggerFactory loggerFactory,
                                          ILightConfiguration<List<MessagingSettings>> messagingSettings) : base(contextFactory, telemetryFactory, loggerFactory, messagingSettings)
        {
        }
    }
}