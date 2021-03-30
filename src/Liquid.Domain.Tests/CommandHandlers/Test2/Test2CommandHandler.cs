using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using MediatR;

namespace Liquid.Domain.Tests.CommandHandlers.Test2
{
    public class Test2CommandHandler : RequestHandlerBase, IRequestHandler<Test2Command, Test2Response>
    {
        public Test2CommandHandler(IMediator mediatorService, 
                                   ILightContext contextService, 
                                   ILightTelemetry telemetryService,
                                   IMapper mapperService) : base(mediatorService, contextService, telemetryService, mapperService)
        {
        }

        public async Task<Test2Response> Handle(Test2Command request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new Test2Response());
        }
    }
}