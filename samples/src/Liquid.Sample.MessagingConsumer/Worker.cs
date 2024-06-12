using Liquid.Messaging;
using Liquid.Messaging.Interfaces;
using Liquid.Sample.Domain.Entities;
using Liquid.Sample.Domain.Handlers.SamplePut;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Sample.MessagingConsumer
{
    public class Worker : ILiquidWorker<SampleMessageEntity>
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMediator _mediator;

        public Worker(ILogger<Worker> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task ProcessMessageAsync(ConsumerMessageEventArgs<SampleMessageEntity> args, CancellationToken cancellationToken)
        {
            await _mediator.Send(new PutCommandRequest(args.Data));
        }
    }
}
