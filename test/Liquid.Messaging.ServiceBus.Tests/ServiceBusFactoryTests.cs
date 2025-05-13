using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Messaging.ServiceBus;
using Liquid.Core.Exceptions;
using Liquid.Messaging.ServiceBus;
using Liquid.Messaging.ServiceBus.Settings;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Liquid.Messaging.ServiceBus.Tests
{
    public class ServiceBusFactoryTests
    {
        private readonly ServiceBusSettings _settings;
        private readonly IOptions<ServiceBusSettings> _options;

        public ServiceBusFactoryTests()
        {
            _settings = new ServiceBusSettings
            {
                Settings = new List<ServiceBusEntitySettings>
                {
                    new ServiceBusEntitySettings
                    {
                        EntityPath = "queue1",
                        ConnectionString = "Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=key",
                        PeekLockMode = true,
                        MaxConcurrentCalls = 5,
                        Subscription = null
                    },
                    new ServiceBusEntitySettings
                    {
                        EntityPath = "topic1",
                        ConnectionString = "Endpoint=sb://test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=key",
                        PeekLockMode = false,
                        MaxConcurrentCalls = 2,
                        Subscription = "sub1"
                    }
                }
            };
            _options = Substitute.For<IOptions<ServiceBusSettings>>();
            _options.Value.Returns(_settings);
        }

        [Fact]
        public void GetSender_ReturnsSender_WhenConfigExists()
        {
            // Arrange
            var factory = new ServiceBusFactory(_options);

            // Act
            var sender = factory.GetSender("queue1");

            // Assert
            Assert.NotNull(sender);
            Assert.IsType<ServiceBusSender>(sender);
        }

        [Fact]
        public void GetSender_ThrowsArgumentOutOfRangeException_WhenConfigMissing()
        {
            // Arrange
            var factory = new ServiceBusFactory(_options);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => factory.GetSender("notfound"));
            Assert.Contains("notfound", ex.Message);
        }

        [Fact]
        public void GetProcessor_ReturnsProcessor_WhenQueueConfigExists()
        {
            // Arrange
            var factory = new ServiceBusFactory(_options);

            // Act
            var processor = factory.GetProcessor("queue1");

            // Assert
            Assert.NotNull(processor);
            Assert.IsType<ServiceBusProcessor>(processor);
        }

        [Fact]
        public void GetProcessor_ReturnsProcessor_WhenTopicConfigExists()
        {
            // Arrange
            var factory = new ServiceBusFactory(_options);

            // Act
            var processor = factory.GetProcessor("topic1");

            // Assert
            Assert.NotNull(processor);
            Assert.IsType<ServiceBusProcessor>(processor);
        }

        [Fact]
        public void GetProcessor_ThrowsMessagingMissingConfigurationException_WhenConfigMissing()
        {
            // Arrange
            var factory = new ServiceBusFactory(_options);

            // Act & Assert
            var ex = Assert.Throws<MessagingMissingConfigurationException>(() => factory.GetProcessor("notfound"));
            Assert.Contains("notfound", ex.Message);
        }

        [Fact]
        public void GetReceiver_ReturnsReceiver_WhenConfigExists()
        {
            // Arrange
            var factory = new ServiceBusFactory(_options);

            // Act
            var receiver = factory.GetReceiver("queue1");

            // Assert
            Assert.NotNull(receiver);
            Assert.IsType<ServiceBusReceiver>(receiver);
        }

        [Fact]
        public void GetReceiver_ThrowsArgumentOutOfRangeException_WhenConfigMissing()
        {
            // Arrange
            var factory = new ServiceBusFactory(_options);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => factory.GetReceiver("notfound"));
            Assert.Contains("notfound", ex.Message);
        }
    }
}
