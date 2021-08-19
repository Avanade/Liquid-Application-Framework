using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Tests.Mock;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.Tests
{
    public class LiquidPipelineTest
    {
        private readonly ILiquidPipeline _sut;
        private readonly LiquidConsumerBase<EntityMock> _consumer;
        public LiquidPipelineTest()
        {
            _sut = new LiquidPipeline();
            _consumer = Substitute.For<LiquidConsumerBase<EntityMock>>(Substitute.For<ILiquidConsumer<EntityMock>>());

        }

        [Fact]
        public async Task Execute_WhenProcessExecutedSucessfully()
        {
            var message = new ProcessMessageEventArgs<EntityMock>();

            message.Data = new EntityMock();
            message.Headers = new Dictionary<string, object>();

            await _sut.Execute(message, _consumer.ProcessMessageAsync, new CancellationToken());

            await _consumer.Received(1).ProcessMessageAsync(Arg.Any<ProcessMessageEventArgs<EntityMock>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public void Execute_WhenProcessFail_ThrowException()
        {
            _consumer.When(x =>
            x.ProcessMessageAsync(Arg.Any<ProcessMessageEventArgs<EntityMock>>(), Arg.Any<CancellationToken>()))
                .Do((call) => throw new Exception());

            var message = new ProcessMessageEventArgs<EntityMock>
            {
                Data = new EntityMock(),
                Headers = new Dictionary<string, object>()
            };

            var task = _sut.Execute(message, _consumer.ProcessMessageAsync, new CancellationToken());

            Assert.ThrowsAsync<MessagingConsumerException>(() => task);
        }
    }
}
