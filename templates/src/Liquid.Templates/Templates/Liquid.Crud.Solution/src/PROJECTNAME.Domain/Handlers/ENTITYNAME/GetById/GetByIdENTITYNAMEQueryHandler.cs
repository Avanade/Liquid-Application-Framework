using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.GetById
{
    public class GetByIdENTITYNAMEQueryHandler : IRequestHandler<GetByIdENTITYNAMEQuery, GetByIdENTITYNAMEQueryResponse>
    {
        private readonly ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> _repository;

        public GetByIdENTITYNAMEQueryHandler(ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }

        ///<inheritdoc/>        
        public async Task<GetByIdENTITYNAMEQueryResponse> Handle(GetByIdENTITYNAMEQuery request, CancellationToken cancellationToken)
        {
            var data = await _repository.FindByIdAsync(request.Id);

            return new GetByIdENTITYNAMEQueryResponse(data);
        }
    }
}
