using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace PROJECTNAME.Domain.Handlers
{
    public class ReadENTITYNAMEHandler : IRequestHandler<ReadENTITYNAMERequest, ReadENTITYNAMEResponse>
    {
        private readonly ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> _repository;

        public ReadENTITYNAMEHandler(ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }

        ///<inheritdoc/>        
        public async Task<ReadENTITYNAMEResponse> Handle(ReadENTITYNAMERequest request, CancellationToken cancellationToken)
        {
            var data = await _repository.FindByIdAsync(request.Id);

            return new ReadENTITYNAMEResponse(data);
        }
    }
}
