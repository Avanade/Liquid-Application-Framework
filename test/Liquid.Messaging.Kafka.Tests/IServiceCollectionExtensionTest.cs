using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Messaging.Kafka.Extensions.DependencyInjection;
using Liquid.Messaging.Kafka.Tests.Mock;
using Liquid.Messaging.Kafka.Tests.Mock.HandlerMock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using System.Linq;
using Xunit;
namespace Liquid.Messaging.Kafka.Tests
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
        public void AddLiquidKafkaProducer_WhenSuccessfullyInjectProducer_GetServicesSucessfully()
        {
            SetCollection();
            _sut.AddLiquidKafkaProducer<MessageMock>("test");

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IKafkaFactory>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidProducer<MessageMock>) && x.Lifetime == ServiceLifetime.Singleton));
        }

        [Fact]
        public void AddLiquidKafkaProducer_WhenSuccessfullyInjectWhitoutTelemetry_GetServicesSucessfully()
        {
            SetCollection();
            _sut.AddLiquidKafkaProducer<MessageMock>("test", false);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IKafkaFactory>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidProducer<MessageMock>) && x.Lifetime == ServiceLifetime.Singleton));
        }

        [Fact]
        public void AddLiquidKafkaConsumer_WhenSuccessfullyInjectConsumer_GetServicesSucessfully()
        {
            SetCollection();
            _sut.AddLiquidKafkaConsumer<WorkerMock, MessageMock>("test");

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IKafkaFactory>());
            Assert.NotNull(provider.GetService<ILiquidWorker<MessageMock>>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(IHostedService)
            && x.ImplementationType == typeof(LiquidBackgroundService<MessageMock>)));
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidConsumer<MessageMock>)
            && x.Lifetime == ServiceLifetime.Singleton));
        }

        [Fact]
        public void AddLiquidKafkaConsumer_WhenSuccessfullyInjectConsumerAndHandlers_GetServicesSucessfully()
        {
            SetCollection();
            _sut.AddLiquidKafkaConsumer<WorkerMediatorMock, MessageMock>("test", false, typeof(MockRequest).Assembly);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IKafkaFactory>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidWorker<MessageMock>)));
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(IHostedService)
            && x.ImplementationType == typeof(LiquidBackgroundService<MessageMock>)));
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidConsumer<MessageMock>)
            && x.Lifetime == ServiceLifetime.Singleton));
        }

    }
}
