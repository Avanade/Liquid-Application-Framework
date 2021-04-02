using System.Collections.Generic;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Gcp.Attributes;
using Liquid.Messaging.Gcp.Tests.Messages;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Gcp.Tests.Producers
{
    /// <summary>
    /// Test Event Producer Class.
    /// </summary>
    /// <seealso cref="PubSubProducer{PubSubTestMessage}" />
    [PubSubProducer("TestPubSub", "TestMessageTopic", true)]
    public class PubSubProducer : PubSubProducer<PubSubTestMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PubSubProducer"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        public PubSubProducer(ILightContextFactory contextFactory,
                              ILightTelemetryFactory telemetryFactory,
                              ILoggerFactory loggerFactory,
                              ILightConfiguration<List<MessagingSettings>> messagingSettings) : base(contextFactory, telemetryFactory, loggerFactory, messagingSettings)
        {
        }
    }
}