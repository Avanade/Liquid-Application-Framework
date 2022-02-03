using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Read
{
    public class GetByIdENTITYNAMEQueryHandler : IRequestHandler<GetByIdENTITYNAMEQuery, GetByIdENTITYNAMEQueryResponse>
    {
        private readonly ILiquidRepository<Entities.ENTITYNAME, int> _repository;

        public GetByIdENTITYNAMEQueryHandler(ILiquidRepository<Entities.ENTITYNAME, int> repository)
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
