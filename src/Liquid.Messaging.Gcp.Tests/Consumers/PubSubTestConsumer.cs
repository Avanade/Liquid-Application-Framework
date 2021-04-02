using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Gcp.Attributes;
using Liquid.Messaging.Gcp.Tests.Messages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Gcp.Tests.Consumers
{
    /// <summary>
    /// Pub/Sub Test Consumer Class.
    /// </summary>
    /// <seealso cref="PubSubConsumer{ServiceBusTestMessage}" />
    [PubSubConsumer("TestPubSub", "TestMessageTopic", "TestMessageSubscription", false)]
    public class PubSubTestConsumer : PubSubConsumer<PubSubTestMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PubSubTestConsumer" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        public PubSubTestConsumer(IServiceProvider serviceProvider,
                                      IMediator mediator,
                                      IMapper mapper,
                                      ILightContextFactory contextFactory,
                                      ILightTelemetryFactory telemetryFactory,
                                      ILoggerFactory loggerFactory,
                                      ILightConfiguration<List<MessagingSettings>> messagingSettings) : base(serviceProvider, mediator, mapper, contextFactory, telemetryFactory, loggerFactory, messagingSettings)
        {
        }


        /// <summary>
        /// Consumes the message from  subscription asynchronous.
        /// </summary>
        /// <param name="message">The message to be consumed.</param>
        /// <param name="headers">The custom headers of message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<bool> ConsumeAsync(PubSubTestMessage message, IDictionary<string, object> headers, CancellationToken cancellationToken)
        {
            PubSubTestMessage.Self = message;
            return await Task.FromResult(true);
        }
    }
}