using Liquid.Messaging.ServiceBus.Tests.Mock;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using NSubstitute;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.ServiceBus.Tests
{
    public class ServiceBusConsumerTest : ServiceBusConsumer<EntityMock>
    {
        private static readonly IServiceBusFactory _factory = Substitute.For<IServiceBusFactory>();

        public ServiceBusConsumerTest() : base(_factory, "test")
        {

        }

        [Fact]
        public void RegisterMessageHandler_WhenRegisteredSucessfully_RegisterMessageHandlerReceivedCall()
        {
            var messageReceiver = Substitute.For<IMessageReceiver>();            
            _factory.GetReceiver(Arg.Any<string>()).Returns(messageReceiver);            

            ProcessMessageAsync += ProcessMessageAsyncMock;

            RegisterMessageHandler();

            messageReceiver.Received(1).RegisterMessageHandler(Arg.Any<Func<Message, CancellationToken, Task>>(), Arg.Any<MessageHandlerOptions>());
        }


        [Fact]
        public async Task MessageHandler_WhenProcessExecutedSucessfully()
        {
            var message = new Message();

            var entity = new EntityMock() { Id = 1, MyProperty = "test" };

            message.Body = JsonSerializer.SerializeToUtf8Bytes(entity);

            var messageReceiver = Substitute.For<IMessageReceiver>();
            _factory.GetReceiver(Arg.Any<string>()).Returns(messageReceiver);

            ProcessMessageAsync += ProcessMessageAsyncMock;

            await MessageHandler(message, new CancellationToken());
        }

        [Fact]
        public async Task MessageHandler_WhenProcessExecutionFail_ThrowException()
        {
            var message = new Message();

            var entity = new EntityMock() { Id = 2, MyProperty = "test" };

            message.Body = JsonSerializer.SerializeToUtf8Bytes(entity);

            var messageReceiver = Substitute.For<IMessageReceiver>();
            _factory.GetReceiver(Arg.Any<string>()).Returns(messageReceiver);

            ProcessMessageAsync += ProcessMessageAsyncMock;

            var task = MessageHandler(message, new CancellationToken());

            await Assert.ThrowsAsync<Exception>(() => task);

        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task ProcessMessageAsyncMock(ProcessMessageEventArgs<EntityMock> args, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (args.Data.Id == 2)
                throw new Exception();
        }

    }
}
