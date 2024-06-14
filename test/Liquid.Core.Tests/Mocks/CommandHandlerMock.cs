using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Core.Tests.Mocks
{
    public class CommandHandlerMock : IRequestHandler<CommandRequestMock>
    {
        public async Task Handle(CommandRequestMock request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
