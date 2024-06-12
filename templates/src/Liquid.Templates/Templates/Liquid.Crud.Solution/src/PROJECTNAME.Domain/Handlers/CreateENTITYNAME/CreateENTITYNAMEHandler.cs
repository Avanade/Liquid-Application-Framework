using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace PROJECTNAME.Domain.Handlers
{
    public class CreateENTITYNAMEHandler : IRequestHandler<CreateENTITYNAMERequest>
    {
        private readonly ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> _repository;

        public CreateENTITYNAMEHandler(ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateENTITYNAMERequest request, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(request.Body);
        }

    }
}
