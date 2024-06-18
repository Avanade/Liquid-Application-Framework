using Confluent.Kafka;
using Liquid.Core.Extensions;
using Liquid.Core.Utils;
using Liquid.Core.Exceptions;
using Liquid.Messaging.Kafka.Settings;
using Liquid.Messaging.Kafka.Tests.Mock;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Liquid.Core.Entities;
using Microsoft.Extensions.Options;

namespace Liquid.Messaging.Kafka.Tests
{
    public class kafkaConsumerTest : KafkaConsumer<MessageMock>
    {
        public static readonly IKafkaFactory _factory = Substitute.For<IKafkaFactory>();
        public static readonly IOptions<KafkaSettings> _settings = GetOptions();

        public static IOptions<KafkaSettings> GetOptions()
        {
            var settings = Substitute.For<IOptions<KafkaSettings>>();
            settings.Value.Returns(new KafkaSettings());
            return settings;
        }
        public kafkaConsumerTest() 
            : base(_factory, _settings)
        {
        }

        [Fact]
        public void RegisterMessageHandler_WhenRegisteredSucessfully_BasicConsumeReceivedCall()
        {
            var messageReceiver = Substitute.For<IConsumer<Ignore, string>>();
            _factory.GetConsumer(Arg.Any<KafkaSettings>()).Returns(messageReceiver);

            ConsumeMessageAsync += ProcessMessageAsyncMock;

            RegisterMessageHandler();

            Assert.True(RegisterHandleMock());
        }

        [Fact]
        public void RegisterMessageHandler_WhenRegistereFail_ThrowException()
        {
            var messageReceiver = Substitute.For<IConsumer<Ignore, string>>();
            _factory.GetConsumer(Arg.Any<KafkaSettings>()).Returns(messageReceiver);

            Assert.Throws<NotImplementedException>(() => RegisterMessageHandler());
        }


        [Fact]
        public async Task MessageHandler_WhenProcessExecutedSucessfully()
        {
            var message = new ConsumeResult<Ignore, string>();

            var entity = new MessageMock() { TestMessageId = 1 };

            var messageObj = new Message<Ignore, string>();
            messageObj.Value = entity.ToJsonString();

            message.Message = messageObj;

            message.Message.Headers = new Headers(); 

            var messageReceiver = Substitute.For<IConsumer<Ignore, string>>();
            _factory.GetConsumer(Arg.Any<KafkaSettings>()).Returns(messageReceiver);

            ConsumeMessageAsync += ProcessMessageAsyncMock;

            var sut =  MessageHandler(message, new CancellationToken());

            await sut;

            Assert.True(sut.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task MessageHandler_WhenProcessExecutionFail_ThrowException()
        {
            var message = new ConsumeResult<Ignore, string>();

            var entity = new MessageMock() { TestMessageId = 2 };

            var messageObj = new Message<Ignore, string>();
            messageObj.Value = entity.ToJsonString();

            message.Message = messageObj;
            message.Message.Headers = new Headers();

            var messageReceiver = Substitute.For<IConsumer<Ignore, string>>();
            _factory.GetConsumer(Arg.Any<KafkaSettings>()).Returns(messageReceiver);

            ConsumeMessageAsync += ProcessMessageAsyncMock;

            var task = MessageHandler(message, new CancellationToken());

            await Assert.ThrowsAsync<MessagingConsumerException>(() => task);

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
