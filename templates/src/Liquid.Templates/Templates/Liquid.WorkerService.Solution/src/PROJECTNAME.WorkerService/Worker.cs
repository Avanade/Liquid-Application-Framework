using Liquid.Messaging;
using Liquid.Messaging.Interfaces;
using PROJECTNAME.Domain.Entities;
using PROJECTNAME.Domain.Handlers.ENTITYNAME.COMMANDNAME;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.WorkerService
{
    public class Worker : ILiquidWorker<ENTITYNAMEEntity>
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMediator _mediator;

        public Worker(ILogger<Worker> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task ProcessMessageAsync(ProcessMessageEventArgs<ENTITYNAMEEntity> args, CancellationToken cancellationToken)
        {

            await _mediator.Send(new COMMANDNAMEENTITYNAMECommand(args.Data));
        }
    }
}
