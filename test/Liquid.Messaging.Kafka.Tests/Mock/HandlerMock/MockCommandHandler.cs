using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Kafka.Tests.Mock.HandlerMock
{
    public class MockCommandHandler : IRequestHandler<MockRequest>
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<Unit> Handle(MockRequest request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return new Unit();
        }
    }
}
