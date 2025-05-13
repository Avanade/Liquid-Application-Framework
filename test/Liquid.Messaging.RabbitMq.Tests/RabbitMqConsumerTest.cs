using Liquid.Core.Entities;
using Liquid.Core.Extensions;
using Liquid.Messaging.RabbitMq.Settings;
using Liquid.Messaging.RabbitMq.Tests.Mock;
using Microsoft.Extensions.Options;
using NSubstitute;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.RabbitMq.Tests
{
    public class RabbitMqConsumerTest : RabbitMqConsumer<MessageMock>
    {
        public static readonly IRabbitMqFactory _factory = Substitute.For<IRabbitMqFactory>();
        public static IOptions<RabbitMqConsumerSettings> _settings = GetOptions();
        public static RabbitMqConsumerSettings _settingsValue;
        public static IOptions<RabbitMqConsumerSettings> GetOptions()
        {
            var settings = Substitute.For<IOptions<RabbitMqConsumerSettings>>();

            _settingsValue = new RabbitMqConsumerSettings
            {
                CompressMessage = true,
                Exchange = "test",
                Queue = "test",
                AdvancedSettings = new AdvancedSettings
                {
                    AutoAck = false,
                    QueueAckModeSettings = new QueueAckModeSettings() { QueueAckMode = QueueAckModeEnum.BasicAck, Requeue = true }
                }
            };
            settings.Value.Returns(_settingsValue);
            return settings;
        }

        public RabbitMqConsumerTest()
            : base(_factory, _settings)
        {
            var model = Substitute.For<IModel>();
            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(model);
        }

        [Fact]
        public void Constructor_WhenFactoryIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RabbitMqConsumer<MessageMock>(null, _settings));
        }

        [Fact]
        public void Constructor_WhenSettingsIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RabbitMqConsumer<MessageMock>(_factory, null));
        }

        [Fact]
        public void Constructor_WhenSettingsValueIsNull_ThrowArgumentNullException()
        {
            var settings = Substitute.For<IOptions<RabbitMqConsumerSettings>>();
            settings.Value.Returns((RabbitMqConsumerSettings)null);
            Assert.Throws<ArgumentNullException>(() => new RabbitMqConsumer<MessageMock>(_factory, settings));
        }


        [Fact]
        public void RegisterMessageHandler_WhenRegisteredSucessfully_BasicConsumeReceivedCall()
        {
            var messageReceiver = Substitute.For<IModel>();
            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(messageReceiver);

            ConsumeMessageAsync += ProcessMessageAsyncMock;

            RegisterMessageHandler();

            Assert.True(RegisterHandleMock());
        }

        [Fact]
        public async Task RegisterMessageHandler_WhenRegistereFail_ThrowException()
        {
            var messageReceiver = Substitute.For<IModel>();
            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(messageReceiver);

            await Assert.ThrowsAsync<NotImplementedException>(() => RegisterMessageHandler());
        }


        [Fact]
        public async Task MessageHandler_WhenProcessExecutedSucessfully()
        {
            var message = new BasicDeliverEventArgs();

            var entity = new MessageMock() { TestMessageId = 1 };

            message.Body = entity.ToJsonBytes();

            var messageReceiver = Substitute.For<IModel>();
            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(messageReceiver);

            ConsumeMessageAsync += ProcessMessageAsyncMock;
            await RegisterMessageHandler();
            await MessageHandler(message, new CancellationToken());
        }

        [Fact]
        public async Task MessageHandler_CallsConsumeMessageAsync_AndAcks_WhenAutoAckIsFalse()
        {
            var message = new BasicDeliverEventArgs();
            var entity = new MessageMock() { TestMessageId = 1 };
            message.Body = entity.ToJsonBytes();
            var messageReceiver = Substitute.For<IModel>();
            ConsumeMessageAsync += ProcessMessageAsyncMock;
            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(messageReceiver);

            await RegisterMessageHandler();

            await MessageHandler(message, new CancellationToken());
            messageReceiver.Received(1).BasicAck(message.DeliveryTag, false);

        }

        [Fact]
        public async Task MessageHandler_CallsConsumeMessageAsync_AndNacks_WhenAutoAckIsFalse()
        {
            var message = new BasicDeliverEventArgs();
            var entity = new MessageMock() { TestMessageId = 2 };
            message.Body = entity.ToJsonBytes();
            var messageReceiver = Substitute.For<IModel>();
            ConsumeMessageAsync += ProcessMessageAsyncMock;
            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(messageReceiver);
            
            await RegisterMessageHandler();
            await MessageHandler(message, new CancellationToken());
            messageReceiver.Received(1).BasicNack(message.DeliveryTag, false, true);
        }

        [Fact]
        public async Task MessageHandler_CallsConsumeMessageAsync_AndRejects_WhenAutoAckIsFalse()
        {
            var message = new BasicDeliverEventArgs();
            var entity = new MessageMock() { TestMessageId = 2 };
            message.Body = entity.ToJsonBytes();
            var messageReceiver = Substitute.For<IModel>();
            ConsumeMessageAsync += ProcessMessageAsyncMock;            

            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(messageReceiver);

            _settingsValue = new RabbitMqConsumerSettings
            {
                CompressMessage = true,
                Exchange = "test",
                Queue = "test",
                AdvancedSettings = new AdvancedSettings
                {
                    AutoAck = false,
                    QueueAckModeSettings = new QueueAckModeSettings() { QueueAckMode = QueueAckModeEnum.BasicReject, Requeue = true }
                }
            };

            _settings.Value.Returns(_settingsValue);

            await RegisterMessageHandler();

            await MessageHandler(message, new CancellationToken());
            messageReceiver.Received(1).BasicReject(message.DeliveryTag, true);
        }
        
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task ProcessMessageAsyncMock(ConsumerMessageEventArgs<MessageMock> args, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (args.Data.TestMessageId == 2)
                throw new Exception();
        }

        private bool RegisterHandleMock()
        {
            try
            {
                RegisterMessageHandler();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
