using Liquid.Core.Exceptions;
using Liquid.Core.Interfaces;
using Liquid.Messaging.RabbitMq.Settings;
using Liquid.Messaging.RabbitMq.Tests.Mock;
using NSubstitute;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.RabbitMq.Tests
{
    public class RabbitMqProducerTest
    {
        private readonly ILiquidProducer<MessageMock> _sut;
        private IRabbitMqFactory _factory;
        private IModel _client;

        public RabbitMqProducerTest()
        {
            _client = Substitute.For<IModel>();

            _factory = Substitute.For<IRabbitMqFactory>();

            _factory.GetSender(Arg.Any<RabbitMqProducerSettings>()).Returns(_client);

            _sut = new RabbitMqProducer<MessageMock>(_factory, new RabbitMqProducerSettings());
        }

        [Fact]
        public async Task SendMessageAsync_WhenMessageSended_BasicPublishReceivedCall()
        {
            var message = new MessageMock();

            await _sut.SendMessageAsync(message);

            _client.Received(1).BasicPublish(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IBasicProperties>(), Arg.Any<ReadOnlyMemory<byte>>());
        }

        [Fact]
        public async Task SendMessageAsync_WhenSendMessageFail_ThrowException()
        {
            var message = new MessageMock();

            _client.When(x => x.BasicPublish(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IBasicProperties>(), Arg.Any<ReadOnlyMemory<byte>>()))
                .Do((call) => throw new Exception());

            var task = _sut.SendMessageAsync(message);

            await Assert.ThrowsAsync<MessagingProducerException>(() => task);
        }

        [Fact]
        public async Task SendMessagesAsync_WhenMessagesSended_BasicPublishReceivedCall()
        {
            var message = new MessageMock();
            var messages = new List<MessageMock>();

            messages.Add(message);
            messages.Add(message);

            await _sut.SendMessagesAsync(messages);

            _client.Received(2).BasicPublish(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IBasicProperties>(), Arg.Any<ReadOnlyMemory<byte>>());
        }

        [Fact]
        public async Task SendMessagesAsync_WhenSendMessagesFail_ThrowException()
        {
            var message = new MessageMock();
            var messages = new List<MessageMock>();

            messages.Add(message);
            messages.Add(message);

            _client.When(x => x.BasicPublish(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IBasicProperties>(), Arg.Any<ReadOnlyMemory<byte>>()))
                .Do((call) => throw new Exception());

            var task = _sut.SendMessagesAsync(messages);

            await Assert.ThrowsAsync<MessagingProducerException>(() => task);
        }

        [Fact]
        public void Ctor_WhenRabbitMqFactoryIsNull_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new RabbitMqProducer<MessageMock>(null, new RabbitMqProducerSettings()));
        }

        [Fact]
        public void Ctor_WhenProducerSettingsIsNull_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new RabbitMqProducer<MessageMock>(_factory, null));
        }
    }
}
