using Liquid.Core.Entities;
using Liquid.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Core.Tests.Mocks
{
    public class WorkerMock : ILiquidWorker<EntityMock>
    {
        public WorkerMock()
        {

        }

        public Task ProcessMessageAsync(ConsumerMessageEventArgs<EntityMock> args, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}
