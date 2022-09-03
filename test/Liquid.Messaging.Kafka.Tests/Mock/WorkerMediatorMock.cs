using Liquid.Messaging.Interfaces;
using Liquid.Messaging.Kafka.Tests.Mock.HandlerMock;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Kafka.Tests.Mock
{
    public class WorkerMediatorMock : ILiquidWorker<MessageMock>
    {
        private readonly IMediator _mediator;

        public WorkerMediatorMock(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task ProcessMessageAsync(ProcessMessageEventArgs<MessageMock> args, CancellationToken cancellationToken)
        {
            await _mediator.Send(new MockRequest(args.Data));
        }
    }
}
