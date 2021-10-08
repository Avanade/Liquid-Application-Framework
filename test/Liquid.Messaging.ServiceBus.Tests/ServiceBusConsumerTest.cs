using AutoFixture.Xunit2;
using Liquid.Messaging.Interfaces;
using Liquid.Messaging.ServiceBus.Tests.Mock;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using NSubstitute;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Messaging.ServiceBus.Tests
{
    public class ServiceBusConsumerTest : ServiceBusConsumer<EntityMock>
    {
        private static readonly IServiceBusFactory _factory = Substitute.For<IServiceBusFactory>();
        private static readonly ILiquidPipeline _pipeline = Substitute.For<ILiquidPipeline>();

        public ServiceBusConsumerTest() : base(_factory, _pipeline)
        {

        }

        [Fact]
        public void Start_WhenClientCreatedSucessfull_RegisterMessageHandlerReceivedCall()
        {
            var messageReceiver = Substitute.For<IMessageReceiver>();
            _factory.GetReceiver(Arg.Any<string>()).Returns(messageReceiver);

            RegisterMessageHandler();

            messageReceiver.Received(1).RegisterMessageHandler(Arg.Any<Func<Message, CancellationToken, Task>>(), Arg.Any<MessageHandlerOptions>());
        }

        [Fact]
        public void Start_WhenClientCreationFail_ThrowException()
        {
            var messageReceiver = Substitute.For<IMessageReceiver>();
            _factory.When(x => x.GetReceiver(Arg.Any<string>())).Do((call) => throw new Exception());

            Assert.Throws<Exception>(() => RegisterMessageHandler());

            messageReceiver.Received(0).RegisterMessageHandler(Arg.Any<Func<Message, CancellationToken, Task>>(), Arg.Any<MessageHandlerOptions>());
        }

        //TODO : review this test case.
        //[Theory, AutoData]
        //public async Task MessageHandler_WhenPipelineExecuteSucessfully_MessageComplete(Message message)
        //{
        //    var entity = new EntityMock() { Id = 1, MyProperty = "test" };
        //    BinaryFormatter bf = new BinaryFormatter();
        //    MemoryStream ms = new MemoryStream();
        //    bf.Serialize(ms, entity);
        //    message.Body = ms.ToArray();

        //    var messageReceiver = Substitute.For<IMessageReceiver>();

        //     messageReceiver.CompleteAsync(Arg.Any<string>()).GetAwaiter().GetResult();

        //    _factory.GetReceiver(Arg.Any<string>()).Returns(messageReceiver);

        //    Start();

        //    await MessageHandler(message, new CancellationToken());

        //    await _pipeline.Received(1).Execute(Arg.Any<ProcessMessageEventArgs<EntityMock>>()
        //        , Arg.Any<Func<ProcessMessageEventArgs<EntityMock>, CancellationToken, Task>>()
        //        , Arg.Any<CancellationToken>());

        //}

        [Theory, AutoData]
        public async Task MessageHandler_WhenPipelineExecutionFail_ThrowException(Message message)
        {
            var entity = new EntityMock() { Id = 1, MyProperty = "test" };

            message.Body = JsonSerializer.SerializeToUtf8Bytes(entity);

            var messageReceiver = Substitute.For<IMessageReceiver>();
            _factory.GetReceiver(Arg.Any<string>()).Returns(messageReceiver);

            _pipeline.When(x => x.Execute(Arg.Any<ProcessMessageEventArgs<EntityMock>>()
                , Arg.Any<Func<ProcessMessageEventArgs<EntityMock>, CancellationToken, Task>>()
                , Arg.Any<CancellationToken>()))
                .Do((call) => throw new Exception());
            RegisterMessageHandler();

            var task = MessageHandler(message, new CancellationToken());

            await Assert.ThrowsAsync<Exception>(() => task);

        }
    }
}
