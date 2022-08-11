using MediatR;

namespace Liquid.Messaging.Kafka.Tests.Mock.HandlerMock
{
    public class MockRequest : IRequest
    {
        public MessageMock Message { get; set; }

        public MockRequest(MessageMock message)
        {
            Message = message;
        }
    }
}
