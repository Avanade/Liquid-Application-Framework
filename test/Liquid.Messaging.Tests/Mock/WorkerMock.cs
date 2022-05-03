using Liquid.Messaging.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Tests.Mock
{
    public class WorkerMock : ILiquidWorker<EntityMock>
    {
        public WorkerMock()
        {

        }

        public Task ProcessMessageAsync(ProcessMessageEventArgs<EntityMock> args, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
