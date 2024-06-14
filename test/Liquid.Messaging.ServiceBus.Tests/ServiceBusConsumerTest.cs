using Azure.Core.Amqp;
using Azure.Messaging.ServiceBus;
using Liquid.Core.Entities;
using Liquid.Core.Exceptions;
using Liquid.Core.Extensions;
using Liquid.Messaging.ServiceBus.Tests.Mock;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.ServiceBus.Tests
{
    public class ServiceBusConsumerTest : ServiceBusConsumer<EntityMock>
    {
        private static readonly IServiceBusFactory _factory = Substitute.For<IServiceBusFactory>();
        private static readonly ServiceBusProcessor _processor = Substitute.For<ServiceBusProcessor>();
        private static readonly ServiceBusReceiver _receiver = Substitute.For<ServiceBusReceiver>();

        public ServiceBusConsumerTest() : base(_factory, "test")
        {
            _factory.GetProcessor(Arg.Any<string>()).Returns(_processor);
        }

        [Fact]
        public void RegisterMessageHandler_WhenRegisteredSucessfully_StartProcessingReceivedCall()
        {
            var messageReceiver = Substitute.For<ServiceBusProcessor>();
            _factory.GetProcessor(Arg.Any<string>()).Returns(messageReceiver);

            ConsumeMessageAsync += ProcessMessageAsyncMock;

            RegisterMessageHandler();

            messageReceiver.Received(1).StartProcessingAsync();
        }

        [Fact]
        public void RegisterMessageHandler_WhenConsumeMessageAssyncIsNull_ThrowNotImplementedException()
        {
            var messageReceiver = Substitute.For<ServiceBusProcessor>();
            _factory.GetProcessor(Arg.Any<string>()).Returns(messageReceiver);

            Assert.Throws<NotImplementedException>(() => RegisterMessageHandler());
        }


        [Fact]
        public async Task MessageHandler_WhenProcessExecutedSucessfully()
        {

            var entity = new EntityMock() { Id = 1, MyProperty = "test" };

            var readOnly = new ReadOnlyMemory<byte>(entity.ToJsonBytes());

            var convertEntity = new List<ReadOnlyMemory<byte>>();

            convertEntity.Add(readOnly);

            var amqp = new AmqpAnnotatedMessage(new AmqpMessageBody(convertEntity));

            BinaryData binary = default;

            var message = ServiceBusReceivedMessage.FromAmqpMessage(amqp, binary);

            var processMessage = new ProcessMessageEventArgs(message, _receiver, new CancellationToken());

            ConsumeMessageAsync += ProcessMessageAsyncMock;

            await MessageHandler(processMessage);
        }


        [Fact]
        public async Task ErrorHandler_WhenProcessErrorExecuted_ThrowsMessagingConsumerException()
        {

            var processError = new ProcessErrorEventArgs(new Exception("teste"), ServiceBusErrorSource.ProcessMessageCallback, "teste"
                , "testqueue", Guid.NewGuid().ToString(), new CancellationToken());

            ProcessErrorAsync += ProcessError;

            await Assert.ThrowsAsync<MessagingConsumerException>(() => ErrorHandler(processError));
        }


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task ProcessMessageAsyncMock(ConsumerMessageEventArgs<EntityMock> args, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

        }

    }
}
