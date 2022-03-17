using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Read
{
    public class ReadENTITYNAMEQueryHandler : IRequestHandler<ReadENTITYNAMEQuery, ReadENTITYNAMEQueryResponse>
    {
        private readonly ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> _repository;

        public ReadENTITYNAMEQueryHandler(ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }

        ///<inheritdoc/>        
        public async Task<ReadENTITYNAMEQueryResponse> Handle(ReadENTITYNAMEQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.FindByIdAsync(request.Id);

            return new ReadENTITYNAMEQueryResponse(data);
        }
    }
}
