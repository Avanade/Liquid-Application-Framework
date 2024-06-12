using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace PROJECTNAME.Domain.Handlers
{
    public class COMMANDNAMEENTITYNAMEHandler : IRequestHandler<COMMANDNAMEENTITYNAMERequest, COMMANDNAMEENTITYNAMEResponse>
    {
        private readonly ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> _repository;

        public COMMANDNAMEENTITYNAMEHandler(ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }


        public async Task<COMMANDNAMEENTITYNAMEResponse> Handle(COMMANDNAMEENTITYNAMERequest request, CancellationToken cancellationToken)
        {
            //TODO: implement handler operation.

            return new COMMANDNAMEENTITYNAMEResponse(request.Body);
        }
    }
}
