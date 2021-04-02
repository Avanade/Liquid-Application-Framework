using System.Collections.Generic;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Aws.Attributes;
using Liquid.Messaging.Aws.Tests.Messages;
using Liquid.Messaging.Configuration;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Aws.Tests.Producers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SnsProducer{SnsTestMessage}" />
    [SnsProducer("TestSns", "SnsTestMessageTopic")]
    public class AwsSnsProducer : SnsProducer<SnsTestMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSnsProducer"/> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        public AwsSnsProducer(ILightContextFactory contextFactory, 
                                  ILightTelemetryFactory telemetryFactory, 
                                  ILoggerFactory loggerFactory, 
                                  ILightConfiguration<List<MessagingSettings>> messagingSettings) : base(contextFactory, telemetryFactory, loggerFactory, messagingSettings)
        {
        }
    }
}