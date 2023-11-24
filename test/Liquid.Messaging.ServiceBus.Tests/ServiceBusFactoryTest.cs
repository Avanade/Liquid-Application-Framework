using Liquid.Messaging.Exceptions;
using Liquid.Messaging.ServiceBus.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NSubstitute.Routing.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.ServiceBus.Tests
{
    public class ServiceBusFactoryTest
    {
        private readonly IOptions<ServiceBusSettings> _configuration;
        private readonly List<ServiceBusEntitySettings> _settings;
        private readonly IServiceBusFactory _sut;

        public ServiceBusFactoryTest()
        {
            _configuration = Substitute.For<IOptions<ServiceBusSettings>>();
            _settings = new List<ServiceBusEntitySettings>();
            _settings.Add(new ServiceBusEntitySettings { EntityPath = "test" });

            _configuration.Value.Returns(new ServiceBusSettings() { Settings = _settings });
            _sut = new ServiceBusFactory(_configuration);
        }

        [Fact]
        public void Ctor_WhenOptionsValueIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ServiceBusFactory(Substitute.For<IOptions<ServiceBusSettings>>()));
        }

        [Fact]
        public void GetProcessor_WhenPeekModeAndConnectionStringIsMissing_ThrowMessagingMissingConfigurationException()
        {
            _settings.Add(new ServiceBusEntitySettings { EntityPath = "test" });
            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetProcessor("test"));

        }
        [Fact]
        public void GetProcessor_WhenReceiveModeAndConnectionStringIsMissing_ThrowMessagingMissingConfigurationException()
        {
            _settings.Add(new ServiceBusEntitySettings { EntityPath = "test", PeekLockMode = false });
            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetProcessor("test"));

        }

        [Fact]
        public void GetReceiver_WhenPeekModeAndConnectionIsMissing_ThrowMessagingMissingConfigurationException()
        {
            _settings.Add(new ServiceBusEntitySettings { EntityPath = "test" });
            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetReceiver("test"));

        }

        [Fact]
        public void GetReceiver_WhenReceiveModeAndConnectionIsMissing_ThrowMessagingMissingConfigurationException()
        {
            _settings.Add(new ServiceBusEntitySettings { EntityPath = "test", PeekLockMode = false });
            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetReceiver("test"));

        }

        [Fact]
        public void GetSender_WhenPeekModeAndConnectionIsMissing_ThrowMessagingMissingConfigurationException()
        {
            _settings.Add(new ServiceBusEntitySettings { EntityPath = "test" });
            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetSender("test"));

        }
        [Fact]
        public void GetSender_WhenReceiveModeAndConnectionIsMissing_ThrowMessagingMissingConfigurationException()
        {
            _settings.Add(new ServiceBusEntitySettings { EntityPath = "test", PeekLockMode = false });
            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetSender("test"));

        }
    }
}
