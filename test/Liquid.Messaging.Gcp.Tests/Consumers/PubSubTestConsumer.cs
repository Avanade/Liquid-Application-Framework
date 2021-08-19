using AutoMapper;
using Liquid.Core.Interfaces;
using Liquid.Messaging.Gcp.Configuration;
using Liquid.Messaging.Gcp.Factories;
using Liquid.Messaging.Gcp.Parameters;
using Liquid.Messaging.Gcp.Tests.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Gcp.Tests.Consumers
{
    /// <summary>
    /// Pub/Sub Test Consumer Class.
    /// </summary>
    /// <seealso cref="PubSubConsumer{ServiceBusTestMessage}" />
    public class PubSubTestConsumer : PubSubConsumer<PubSubTestMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PubSubTestConsumer"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="pubSubClientFactory">The pub sub client factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="pubSubConsumerParameter">The pub sub consumer parameter.</param>
        public PubSubTestConsumer(IServiceProvider serviceProvider,
                                  IMediator mediator,
                                  IMapper mapper,
                                  ILiquidContext contextFactory,
                                  ILoggerFactory loggerFactory,
                                  IPubSubClientFactory pubSubClientFactory,
                                  ILiquidConfiguration<PubSubSettings> messagingConfiguration,
                                  PubSubConsumerParameter pubSubConsumerParameter) : base(serviceProvider, mediator, mapper, contextFactory, loggerFactory, pubSubClientFactory, messagingConfiguration, pubSubConsumerParameter)
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