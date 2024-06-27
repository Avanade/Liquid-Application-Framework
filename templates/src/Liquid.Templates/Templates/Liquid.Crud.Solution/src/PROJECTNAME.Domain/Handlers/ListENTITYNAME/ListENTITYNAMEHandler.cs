using Liquid.Core.Interfaces;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace PROJECTNAME.Domain.Handlers
{
    public class ListENTITYNAMEHandler : IRequestHandler<ListENTITYNAMERequest, ListENTITYNAMEResponse>
    {
        private readonly ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> _repository;

        public ListENTITYNAMEHandler(ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }

        ///<inheritdoc/>        
        public async Task<ListENTITYNAMEResponse> Handle(ListENTITYNAMERequest request, CancellationToken cancellationToken)
        {
            var data = await _repository.FindAllAsync();

            return new ListENTITYNAMEResponse(data);
        }
    }
}
