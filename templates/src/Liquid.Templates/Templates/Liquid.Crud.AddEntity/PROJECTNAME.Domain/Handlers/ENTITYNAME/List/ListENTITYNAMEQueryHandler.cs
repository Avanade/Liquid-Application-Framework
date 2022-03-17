using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.List
{
    public class ListENTITYNAMEQueryHandler : IRequestHandler<ListENTITYNAMEQuery, ListENTITYNAMEQueryResponse>
    {
        private readonly ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> _repository;

        public ListENTITYNAMEQueryHandler(ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }

        ///<inheritdoc/>        
        public async Task<ListENTITYNAMEQueryResponse> Handle(ListENTITYNAMEQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.FindAllAsync();

            return new ListENTITYNAMEQueryResponse(data);
        }
    }
}
