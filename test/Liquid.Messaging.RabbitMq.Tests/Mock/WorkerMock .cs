using Liquid.Messaging.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.RabbitMq.Tests.Mock
{
    public class WorkerMock : ILiquidWorker<MessageMock>
    {
        public async Task ProcessMessageAsync(ProcessMessageEventArgs<MessageMock> args, CancellationToken cancellationToken)
        {
            return;
        }
    }
}
