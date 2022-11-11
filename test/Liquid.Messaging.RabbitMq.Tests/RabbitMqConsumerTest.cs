using Liquid.Core.Extensions;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.RabbitMq.Settings;
using Liquid.Messaging.RabbitMq.Tests.Mock;
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
        public RabbitMqConsumerTest()
            : base(_factory, new RabbitMqConsumerSettings())
        {

        }

        [Fact]
        public void RegisterMessageHandler_WhenRegisteredSucessfully_BasicConsumeReceivedCall()
        {
            var messageReceiver = Substitute.For<IModel>();
            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(messageReceiver);

            ProcessMessageAsync += ProcessMessageAsyncMock;

            RegisterMessageHandler();

            Assert.True(RegisterHandleMock());
        }

        [Fact]
        public void RegisterMessageHandler_WhenRegistereFail_ThrowException()
        {
            var messageReceiver = Substitute.For<IModel>();
            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(messageReceiver);

            Assert.Throws<NotImplementedException>(() => RegisterMessageHandler());
        }


        [Fact]
        public async Task MessageHandler_WhenProcessExecutedSucessfully()
        {
            var message = new BasicDeliverEventArgs();

            var entity = new MessageMock() { TestMessageId = 1 };

            message.Body = entity.ToJsonBytes();

            var messageReceiver = Substitute.For<IModel>();
            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(messageReceiver);

            ProcessMessageAsync += ProcessMessageAsyncMock;

            await MessageHandler(message, new CancellationToken());
        }

        [Fact]
        public async Task MessageHandler_WhenProcessExecutionFail_ThrowException()
        {
            var message = new BasicDeliverEventArgs();

            var entity = new MessageMock() { TestMessageId = 2 };

            message.Body = entity.ToJsonBytes();

            var messageReceiver = Substitute.For<IModel>();
            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(messageReceiver);

            ProcessMessageAsync += ProcessMessageAsyncMock;

            var task = MessageHandler(message, new CancellationToken());

            await Assert.ThrowsAsync<MessagingConsumerException>(() => task);

        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task ProcessMessageAsyncMock(ProcessMessageEventArgs<MessageMock> args, CancellationToken cancellationToken)
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
