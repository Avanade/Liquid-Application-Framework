using Liquid.Messaging.Tests.Mock;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.Tests
{
    public class LiquidConsumerBaseTest : LiquidConsumerBase<EntityMock>
    {
        private static readonly Mock<ILiquidConsumer<EntityMock>> _consumer = new Mock<ILiquidConsumer<EntityMock>>();

        public LiquidConsumerBaseTest() : base(_consumer.Object)
        {
        }

        [Fact]
        public async Task ExecuteAsync_WhenStart_ConsumerReceivedStartCall()
        {
            _consumer.Setup(x => x.RegisterMessageHandler()).Verifiable();
           
            base.ExecuteAsync(new CancellationToken()).Wait(3000);

            _consumer.Verify(x => x.RegisterMessageHandler());
        }


        [Fact]
        public async Task ExecuteAsync_WhenStartFail_ThrowException()
        {
            _consumer.Setup(x =>
            x.RegisterMessageHandler()).Throws(new Exception());

            var task = ExecuteAsync(new CancellationToken());

            await Assert.ThrowsAsync<Exception>(() => task);
        }

        public override Task ProcessMessageAsync(ProcessMessageEventArgs<EntityMock> args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
