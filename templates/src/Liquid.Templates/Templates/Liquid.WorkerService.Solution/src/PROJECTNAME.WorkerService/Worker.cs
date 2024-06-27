using Liquid.Core.Entities;
using Liquid.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using PROJECTNAME.Domain.Entities;
using PROJECTNAME.Domain.Handlers;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.WorkerService
{
    public class Worker : ILiquidWorker<ENTITYNAME>
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMediator _mediator;

        public Worker(ILogger<Worker> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task ProcessMessageAsync(ConsumerMessageEventArgs<ENTITYNAME> args, CancellationToken cancellationToken)
        {

            await _mediator.Send(new COMMANDNAMEENTITYNAMERequest(args.Data));
        }
    }
}
