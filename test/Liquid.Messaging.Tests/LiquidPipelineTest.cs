using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Interfaces;
using Liquid.Messaging.Tests.Mock;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly LiquidBackgroundService<EntityMock> _consumer;
        public LiquidPipelineTest()
        {
            var worker = Substitute.For<ILiquidWorker<EntityMock>>();

            var services = new ServiceCollection();

            services.AddSingleton(worker);

            var provider = services.BuildServiceProvider();

            _consumer = Substitute.For<LiquidBackgroundService<EntityMock>>(provider, Substitute.For<ILiquidConsumer<EntityMock>>());

            _sut = new LiquidPipeline();
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
