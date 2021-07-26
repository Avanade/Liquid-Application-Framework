using AutoMapper;
using Liquid.Core.Interfaces;
using Liquid.Messaging.RabbitMq.Configuration;
using Liquid.Messaging.RabbitMq.Parameters;
using Liquid.Messaging.RabbitMq.Tests.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.RabbitMq.Tests.Consumers
{
    /// <summary>
    /// RabbitMqTestEventConsumer Class.
    /// </summary>
    /// <seealso>
    ///     <cref>Liquid.Messaging.RabbitMq.RabbitMqConsumer{Liquid.Messaging.RabbitMq.Tests.Messages.RabbitMqTestMessage}</cref>
    /// </seealso>
    public class RabbitMqTestEventConsumer : RabbitMqConsumer<RabbitMqTestMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqTestEventConsumer"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="rabbitMqConsumerParameter">The rabbit mq consumer parameter.</param>
        public RabbitMqTestEventConsumer(IServiceProvider serviceProvider, 
                                         IMediator mediator, 
                                         IMapper mapper, 
                                         ILiquidContext contextFactory, 
                                         ILoggerFactory loggerFactory, 
                                         ILiquidConfiguration<RabbitMqSettings> messagingConfiguration, 
                                         RabbitMqConsumerParameter rabbitMqConsumerParameter) : base(serviceProvider, mediator, mapper, contextFactory, loggerFactory, messagingConfiguration, rabbitMqConsumerParameter)
        {
        }


        /// <summary>
        /// Consumes the message from  subscription asynchronous.
        /// </summary>
        /// <param name="message">The message to be consumed.</param>
        /// <param name="headers">The custom headers of message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<bool> ConsumeAsync(RabbitMqTestMessage message, IDictionary<string, object> headers, CancellationToken cancellationToken)
        {
            RabbitMqTestMessage.Self = message;
            return await Task.FromResult(true);
        }
    }
}