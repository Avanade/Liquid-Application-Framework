using Azure.Messaging.ServiceBus;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Interfaces;
using Liquid.Messaging.ServiceBus.Tests.Mock;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.ServiceBus.Tests
{
    public class ServiceBusProducerTest
    {
        private readonly ILiquidProducer<EntityMock> _sut;
        private readonly IServiceBusFactory _serviceBusFactory;
        private readonly ServiceBusSender _client;
        private readonly EntityMock _message;

        public ServiceBusProducerTest()
        {
            _client = Substitute.For<ServiceBusSender>();

            _serviceBusFactory = Substitute.For<IServiceBusFactory>();

            _serviceBusFactory.GetSender(Arg.Any<string>()).Returns(_client);

            _sut = new ServiceBusProducer<EntityMock>(_serviceBusFactory, "test");

            _message = new EntityMock() { Id = 1, MyProperty = "test" };
        }

        [Fact]
        public async Task SendAsync_WhenSingleEntitySendedSuccessfully_ClientReceivedCall()
        {
            var customProperties = new Dictionary<string, object>();
            customProperties.Add("test", 123);

            await _sut.SendMessageAsync(_message, customProperties);

            await _client.Received(1).SendMessageAsync(Arg.Any<ServiceBusMessage>());

        }

        [Fact]
        public async Task SendAsync_WhenListEntitiesSendedSuccessfully_ClientReceivedCall()
        {
            var entities = new List<EntityMock>() { _message };

            await _sut.SendMessagesAsync(entities);

            await _client.Received(1).SendMessagesAsync(Arg.Any<IList<ServiceBusMessage>>());
        }

        [Fact]
        public async Task SendAsync_WhenSingleEntitySendFailed_ThrowError()
        {
            _client.When(x => x.SendMessageAsync(Arg.Any<ServiceBusMessage>())).Do((call) => throw new Exception());

            var sut = _sut.SendMessageAsync(_message, new Dictionary<string, object>());

            await Assert.ThrowsAsync<MessagingProducerException>(() => sut);
        }

        [Fact]
        public async Task SendAsync_WhenListEntitiesSendFailed_ThrowError()
        {
            var entities = new List<EntityMock>() { _message };

            _client.When(x => x.SendMessagesAsync(Arg.Any<IList<ServiceBusMessage>>())).Do((call) => throw new Exception());

            var sut = _sut.SendMessagesAsync(entities);

            await Assert.ThrowsAsync<MessagingProducerException>(() => sut);
        }


    }
}
