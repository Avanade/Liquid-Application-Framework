using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.COMMANDNAME
{
    public class COMMANDNAMEENTITYNAMECommandHandler : IRequestHandler<COMMANDNAMEENTITYNAMECommand, COMMANDNAMEENTITYNAMECommandResponse>
    {
        private readonly ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> _repository;

        public COMMANDNAMEENTITYNAMECommandHandler(ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }


        public async Task<COMMANDNAMEENTITYNAMECommandResponse> Handle(COMMANDNAMEENTITYNAMECommand request, CancellationToken cancellationToken)
        {
            //TODO: implement handler operation.

            return new COMMANDNAMEENTITYNAMECommandResponse(request.Body);
        }
    }
}
