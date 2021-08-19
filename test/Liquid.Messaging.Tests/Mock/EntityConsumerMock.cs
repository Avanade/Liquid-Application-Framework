using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Tests.Mock
{
    public class EntityConsumerMock : LiquidConsumerBase<EntityMock>
    {
        public EntityConsumerMock(ILiquidConsumer<EntityMock> consumer) : base(consumer)
        {
        }

        public override async Task ProcessMessageAsync(ProcessMessageEventArgs<EntityMock> args, CancellationToken cancellationToken)
        {

        }
    }
}
