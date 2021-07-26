using AutoMapper;
using Liquid.Core.Interfaces;
using Liquid.Messaging.Aws.Configuration;
using Liquid.Messaging.Aws.Parameters;
using Liquid.Messaging.Aws.Tests.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Aws.Tests.Consumers
{
    /// <summary>
    /// AwsSqsConsumer Class.
    /// </summary>
    /// <seealso>
    ///     <cref>Liquid.Messaging.Aws.AwsSqsConsumer{Liquid.Messaging.Aws.Tests.Messages.SqsTestMessage}</cref>
    /// </seealso>
    public class AwsSqsConsumer : SqsConsumer<SqsTestMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSqsConsumer"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="sqsConsumerParameter">The SQS consumer parameter.</param>
        public AwsSqsConsumer(IServiceProvider serviceProvider, 
                              IMediator mediator, 
                              IMapper mapper, 
                              ILiquidContext contextFactory,  
                              ILoggerFactory loggerFactory, 
                              ILiquidConfiguration<AwsMessagingSettings> messagingConfiguration, 
                              SqsConsumerParameter sqsConsumerParameter) : base(serviceProvider, mediator, mapper, contextFactory, loggerFactory, messagingConfiguration, sqsConsumerParameter)
        {
        }

        /// <summary>
        /// Consumes the message from  subscription asynchronous.
        /// </summary>
        /// <param name="message">The message to be consumed.</param>
        /// <param name="headers">The custom headers of message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public override async Task<bool> ConsumeAsync(SqsTestMessage message, IDictionary<string, object> headers, CancellationToken cancellationToken)
        {
            SqsTestMessage.Self = message;
            return await Task.FromResult(true);
        }
    }
}