using Liquid.Core.Utils;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.RabbitMq.Settings;
using Liquid.Messaging.RabbitMq.Tests.Mock;
using Microsoft.Extensions.Logging;
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
        public static readonly ILogger<RabbitMqConsumer<MessageMock>> _logger = Substitute.For<ILogger<RabbitMqConsumer<MessageMock>>>();

        public RabbitMqConsumerTest()
            : base(_factory, new RabbitMqConsumerSettings(), _logger)
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
        public void MessageHandler_WhenProcessExecutionFail_LogException()
        {
            var message = new BasicDeliverEventArgs();

            var entity = new MessageMock() { TestMessageId = 2 };

            message.Body = entity.ToJsonBytes();

            var messageReceiver = Substitute.For<IModel>();
            _factory.GetReceiver(Arg.Any<RabbitMqConsumerSettings>()).Returns(messageReceiver);

            ProcessMessageAsync += ProcessMessageAsyncMock;

            var task = MessageHandler(message, new CancellationToken());

            _logger.Received(1);

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
