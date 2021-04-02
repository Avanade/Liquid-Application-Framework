using System.Collections.Generic;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Kafka.Attributes;
using Liquid.Messaging.Kafka.Tests.Messages;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Kafka.Tests.Producers
{
    /// <summary>
    /// Test Event Producer Class.
    /// </summary>
    /// <seealso cref="KafkaProducer{KafkaTestMessage}" />
    [KafkaProducer("TestKafka", "TestMessageTopic", true)]
    public class KafkaTestProducer : KafkaProducer<KafkaTestMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaTestProducer"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        public KafkaTestProducer(ILightContextFactory contextFactory,
                              ILightTelemetryFactory telemetryFactory,
                              ILoggerFactory loggerFactory,
                              ILightConfiguration<List<MessagingSettings>> messagingSettings) : base(contextFactory, telemetryFactory, loggerFactory, messagingSettings)
        {
        }
    }
}