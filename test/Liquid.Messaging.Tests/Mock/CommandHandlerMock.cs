using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Tests.Mock
{
    public class CommandHandlerMock : IRequestHandler<CommandRequestMock>
    {
        public async Task<Unit> Handle(CommandRequestMock request, CancellationToken cancellationToken)
        {
            return new Unit();
        }
    }
}
