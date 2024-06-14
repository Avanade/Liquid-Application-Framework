using Confluent.Kafka;
using Liquid.Core.Exceptions;
using Liquid.Core.Interfaces;
using Liquid.Messaging.Kafka.Settings;
using Liquid.Messaging.Kafka.Tests.Mock;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.Kafka.Tests
{
    public class KafkaProducerTest
    {
        private readonly ILiquidProducer<MessageMock> _sut;
        private IKafkaFactory _factory;
        private IProducer<Null, string> _client;

        public KafkaProducerTest()
        {
            _client = Substitute.For<IProducer<Null, string>>();

            _factory = Substitute.For<IKafkaFactory>();

            _factory.GetProducer(Arg.Any<KafkaSettings>()).Returns(_client);

            _sut = new KafkaProducer<MessageMock>(new KafkaSettings(), _factory);
        }

        [Fact]
        public async Task SendMessageAsync_WhenMessageSended_ClientReceivedCall()
        {
            var message = new MessageMock();

            await _sut.SendMessageAsync(message);

            await _client.Received(1).ProduceAsync(Arg.Any<string>(), Arg.Any<Message<Null, string>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task SendMessageAsync_WhenSendMessageFail_ThrowException()
        {
            var message = new MessageMock();

            _client.When(x => x.ProduceAsync(Arg.Any<string>(), Arg.Any<Message<Null, string>>(), Arg.Any<CancellationToken>()))
                .Do((call) => throw new Exception());

            var task = _sut.SendMessageAsync(message);

            await Assert.ThrowsAsync<MessagingProducerException>(() => task);
        }

        [Fact]
        public async Task SendMessagesAsync_WhenMessagesSended_ClientReceivedCall()
        {
            var message = new MessageMock();
            var messages = new List<MessageMock>();

            messages.Add(message);
            messages.Add(message);

            await _sut.SendMessagesAsync(messages);

            await _client.Received(2).ProduceAsync(Arg.Any<string>(), Arg.Any<Message<Null, string>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task SendMessagesAsync_WhenSendMessagesFail_ThrowException()
        {
            var message = new MessageMock();
            var messages = new List<MessageMock>();

            messages.Add(message);
            messages.Add(message);

            _client.When(x => x.ProduceAsync(Arg.Any<string>(), Arg.Any<Message<Null, string>>(), Arg.Any<CancellationToken>()))
                .Do((call) => throw new Exception());

            var task = _sut.SendMessagesAsync(messages);

            await Assert.ThrowsAsync<MessagingProducerException>(() => task);
        }

        [Fact]
        public void Ctor_WhenRabbitMqFactoryIsNull_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new KafkaProducer<MessageMock>(new KafkaSettings(), null));
        }
    }
}
