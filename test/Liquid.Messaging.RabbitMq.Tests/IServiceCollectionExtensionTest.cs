using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Messaging.RabbitMq.Extensions.DependencyInjection;
using Liquid.Messaging.RabbitMq.Tests.Mock;
using Liquid.Messaging.RabbitMq.Tests.Mock.HandlerMock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using System.Linq;
using Xunit;

namespace Liquid.Messaging.RabbitMq.Tests
{
    public class IServiceCollectionExtensionTest
    {
        private IServiceCollection _sut;
        private IConfiguration _configProvider = Substitute.For<IConfiguration>();
        private IConfigurationSection _configurationSection = Substitute.For<IConfigurationSection>();

        private void SetCollection()
        {
            _configProvider.GetSection(Arg.Any<string>()).Returns(_configurationSection);
            _sut = new ServiceCollection();
            _sut.AddSingleton(_configProvider);
        }

        [Fact]
        public void AddLiquidRabbitMqProducer_WhenSuccessfullyInjectProducer_GetServicesSucessfully()
        {
            SetCollection();
            _sut.AddLiquidRabbitMqProducer<MessageMock>("test");

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IRabbitMqFactory>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidProducer<MessageMock>) && x.Lifetime == ServiceLifetime.Singleton));
        }

        [Fact]
        public void AddLiquidRabbitMqProducer_WhenSuccessfullyInjectWhitoutTelemetry_GetServicesSucessfully()
        {
            SetCollection();
            _sut.AddLiquidRabbitMqProducer<MessageMock>("test", false);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IRabbitMqFactory>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidProducer<MessageMock>) && x.Lifetime == ServiceLifetime.Singleton));
        }

        [Fact]
        public void AddLiquidRabbitMqConsumer_WhenSuccessfullyInjectConsumer_GetServicesSucessfully()
        {
            SetCollection();
            _sut.AddLiquidRabbitMqConsumer<WorkerMock, MessageMock>("test");

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IRabbitMqFactory>());
            Assert.NotNull(provider.GetService<ILiquidWorker<MessageMock>>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(IHostedService)
            && x.ImplementationType == typeof(LiquidBackgroundService<MessageMock>)));
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidConsumer<MessageMock>)
            && x.Lifetime == ServiceLifetime.Singleton));
        }

        [Fact]
        public void AddLiquidRabbitMqConsumer_WhenSuccessfullyInjectConsumerAndHandlers_GetServicesSucessfully()
        {
            SetCollection();
            _sut.AddLiquidRabbitMqConsumer<WorkerMediatorMock, MessageMock>("test", false, typeof(MockRequest).Assembly);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IRabbitMqFactory>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidWorker<MessageMock>)));
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(IHostedService)
            && x.ImplementationType == typeof(LiquidBackgroundService<MessageMock>)));
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidConsumer<MessageMock>)
            && x.Lifetime == ServiceLifetime.Singleton));
        }

    }
}
