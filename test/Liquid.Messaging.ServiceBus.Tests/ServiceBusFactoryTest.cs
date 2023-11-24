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
        private readonly IServiceBusFactory _sut;

        public ServiceBusFactoryTest()
        {
            _configuration = Substitute.For<IOptions<ServiceBusSettings>>();
            var settings = new List<ServiceBusEntitySettings>();
            settings.Add(new ServiceBusEntitySettings { EntityPath = "test" });

            _configuration.Value.Returns(new ServiceBusSettings() { Settings = settings });
            _sut = new ServiceBusFactory(_configuration);
        }

        [Fact]
        public void GetProcessor_WhenConfigurationIsMissing_ThrowMessagingMissingConfigurationException()
        {            

            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetProcessor("test"));

        }

        [Fact]
        public void GetReceiver_WhenConfigurationIsMissing_ThrowMessagingMissingConfigurationException()
        {

            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetReceiver("test"));

        }

        [Fact]
        public void GetSender_WhenConfigurationIsMissing_ThrowMessagingMissingConfigurationException()
        {

            Assert.Throws<MessagingMissingConfigurationException>(() => _sut.GetSender("test"));

        }
    }
}
