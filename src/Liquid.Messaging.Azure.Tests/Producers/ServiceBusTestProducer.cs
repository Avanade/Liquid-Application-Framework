using System.Collections.Generic;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Azure.Attributes;
using Liquid.Messaging.Azure.Tests.Messages;
using Liquid.Messaging.Configuration;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Azure.Tests.Producers
{
    /// <summary>
    /// Test Event Producer Class.
    /// </summary>
    /// <seealso cref="ServiceBusProducer{ServiceBusTestMessage}" />
    [ServiceBusProducer("TestAzureServiceBus", "TestMessageTopic")]
    public class ServiceBusTestProducer : ServiceBusProducer<ServiceBusTestMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusTestProducer"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        public ServiceBusTestProducer(ILightContextFactory contextFactory,
                                       ILightTelemetryFactory telemetryFactory,
                                       ILoggerFactory loggerFactory,
                                       ILightConfiguration<List<MessagingSettings>> messagingSettings) : base(contextFactory, telemetryFactory, loggerFactory, messagingSettings)
        {
        }
    }
}