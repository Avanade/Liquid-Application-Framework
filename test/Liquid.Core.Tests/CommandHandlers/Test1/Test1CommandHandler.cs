using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

namespace Liquid.Domain.Tests.CommandHandlers.Test1
{
    public class Test1CommandHandler : IRequestHandler<Test1Command, Test1Response>
    {
        public async Task<Test1Response> Handle(Test1Command request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new Test1Response());
        }
    }
}