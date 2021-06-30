using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace Liquid.Domain.Tests.CommandHandlers.Test2
{
    public class Test2CommandHandler : IRequestHandler<Test2Command, Test2Response>
    {
        public async Task<Test2Response> Handle(Test2Command request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new Test2Response());
        }
    }
}