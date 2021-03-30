using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using MediatR;

namespace Liquid.Domain.Tests.CommandHandlers.Test1
{
    public class Test1CommandHandler : RequestHandlerBase, IRequestHandler<Test1Command, Test1Response>
    {
        public Test1CommandHandler(IMediator mediatorService, 
                                   ILightContext contextService, 
                                   ILightTelemetry telemetryService,
                                   IMapper mapperService) : base(mediatorService, contextService, telemetryService, mapperService)
        {
        }

        public async Task<Test1Response> Handle(Test1Command request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new Test1Response());
        }
    }
}