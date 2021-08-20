using Liquid.Messaging.Tests.Mock;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.Tests
{
    public class LiquidConsumerBaseTest : LiquidConsumerBase<EntityMock>
    {
        static readonly CancellationTokenSource s_cts = new CancellationTokenSource();
        private readonly LiquidConsumerBase<EntityMock> _sut;
        private static readonly ILiquidConsumer<EntityMock> _consumer = Substitute.For<ILiquidConsumer<EntityMock>>();

        public LiquidConsumerBaseTest() : base(SubstituteConsumer())
        {            
            _sut = new EntityConsumerMock(_consumer);
        }
        
        public static ILiquidConsumer<EntityMock> SubstituteConsumer()
        {              
            return _consumer;
        }

        [Fact]
        public void ExecuteAsync_WhenStart_ConsumerReceivedStartCall()
        {
            _ = base.ExecuteAsync(new CancellationToken());

            _consumer.Received().Start();
        }


        [Fact]
        public async Task ExecuteAsync_WhenStartFail_ThrowException()
        {
            _consumer.When(x =>
            x.Start())
                .Do((call) => throw new Exception());

            var task = ExecuteAsync(new CancellationToken());

            await Assert.ThrowsAsync<Exception>(() => task);
        }

        public override Task ProcessMessageAsync(ProcessMessageEventArgs<EntityMock> args, CancellationToken cancellationToken)
        {
            return _sut.ProcessMessageAsync(args, cancellationToken);
        }
    }

}
