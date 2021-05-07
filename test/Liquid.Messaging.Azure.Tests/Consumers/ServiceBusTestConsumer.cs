using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Messaging.Azure.Configuration;
using Liquid.Messaging.Azure.Parameters;
using Liquid.Messaging.Azure.Tests.Messages;
using Liquid.Messaging.Configuration;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Azure.Tests.Consumers
{
    /// <summary>
    /// AzureServiceBusTestEventConsumer Class.
    /// </summary>
    /// <seealso cref="ServiceBusConsumer{AzureTestMessage}" />
    public class ServiceBusTestConsumer : ServiceBusConsumer<ServiceBusTestMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusTestConsumer"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The service bus messaging configuration.</param>
        /// <param name="serviceBusConsumerParameter">The service bus consumer parameter.</param>
        public ServiceBusTestConsumer(IServiceProvider serviceProvider, 
                                      IMediator mediator, 
                                      IMapper mapper, 
                                      ILightContextFactory contextFactory, 
                                      ILightTelemetryFactory telemetryFactory, 
                                      ILoggerFactory loggerFactory, 
                                      ILightMessagingConfiguration<ServiceBusSettings> messagingConfiguration, 
                                      ServiceBusConsumerParameter serviceBusConsumerParameter) : base(serviceProvider, mediator, mapper, contextFactory, telemetryFactory, loggerFactory, messagingConfiguration, serviceBusConsumerParameter)
        {
        }


        /// <inheritdoc/>
        public override async Task<bool> ConsumeAsync(ServiceBusTestMessage message, IDictionary<string, object> headers, CancellationToken cancellationToken)
        {
            ServiceBusTestMessage.Self = message;
            return await Task.FromResult(true);
        }
    }
}