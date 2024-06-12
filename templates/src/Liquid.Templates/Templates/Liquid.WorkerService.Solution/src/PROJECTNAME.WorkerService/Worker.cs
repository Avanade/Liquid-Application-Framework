using Liquid.Messaging;
using Liquid.Messaging.Interfaces;
using PROJECTNAME.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using PROJECTNAME.Domain.Handlers;

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
