using Liquid.Messaging.Interfaces;
using Liquid.Messaging.Tests.Mock;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.Tests
{
    public class LiquidBackgroundServiceTest : LiquidBackgroundService<EntityMock>
    {
        private static ILiquidConsumer<EntityMock> _consumer = Substitute.For<ILiquidConsumer<EntityMock>>();
        private static IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();

        public LiquidBackgroundServiceTest() : base(_serviceProvider, _consumer)
        {

        }

        [Fact]
        public void ExecuteAsync_WhenStart_ConsumerReceivedStartCall()
        {
            var task = base.ExecuteAsync(new CancellationToken());

            _consumer.Received().RegisterMessageHandler();
        }

        [Fact]
        public async Task ExecuteAsync_WhenStartFail_ThrowException()
        {
            _consumer.When(x =>
            x.RegisterMessageHandler())
                .Do((call) => throw new Exception());

            var task = ExecuteAsync(new CancellationToken());

            await Assert.ThrowsAsync<Exception>(() => task);
        }

        [Fact]
        public async Task ProcessMessageAsync_WhenMessageProcessedSuccessfuly_WorkerReceiveCall()
        {
            var worker = Substitute.For<ILiquidWorker<EntityMock>>();

            var sut = new LiquidBackgroundService<EntityMock>(GetProvider(worker), _consumer);

            await sut.ProcessMessageAsync(new ProcessMessageEventArgs<EntityMock>(), new CancellationToken());

            await worker.Received(1).ProcessMessageAsync(Arg.Any<ProcessMessageEventArgs<EntityMock>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ProcessMessageAsync_WhenMessageProcessedWithError_ThrowException()
        {
            var worker = Substitute.For<ILiquidWorker<EntityMock>>();
            worker.When(x => x.ProcessMessageAsync(Arg.Any<ProcessMessageEventArgs<EntityMock>>(), Arg.Any<CancellationToken>()))
                .Do((call) => throw new Exception());

            var sut = new LiquidBackgroundService<EntityMock>(GetProvider(worker), _consumer);

            await Assert.ThrowsAsync<Exception>(() => sut.ProcessMessageAsync(new ProcessMessageEventArgs<EntityMock>(), new CancellationToken()));
        }
        private static ServiceProvider GetProvider(ILiquidWorker<EntityMock> worker)
        {
            var services = new ServiceCollection();

            services.AddSingleton(worker);

            return services.BuildServiceProvider();
        }

    }

}
