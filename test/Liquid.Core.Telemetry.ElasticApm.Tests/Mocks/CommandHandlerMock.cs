using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Liquid.Core.Telemetry.ElasticApm.Tests.Mocks
{
    public sealed class CommandHandlerMock : IRequestHandler<RequestMock, ResponseMock>
    {
        public Task<ResponseMock> Handle(RequestMock request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
