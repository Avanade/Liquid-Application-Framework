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
        public static readonly IOptions<RabbitMqConsumerSettings> _settings = GetOptions();

        public static IOptions<RabbitMqConsumerSettings> GetOptions()
        {
            var settings = Substitute.For<IOptions<RabbitMqConsumerSettings>>();
            settings.Value.Returns(new RabbitMqConsumerSettings());
            return settings;
        }

        public RabbitMqConsumerTest()
            : base(_factory, _settings)
        {
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

            await MessageHandler(message, new CancellationToken());
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
