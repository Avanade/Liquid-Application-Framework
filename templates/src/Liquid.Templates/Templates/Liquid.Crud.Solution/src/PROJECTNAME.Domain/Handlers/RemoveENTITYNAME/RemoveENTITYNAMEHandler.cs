using Liquid.Core.Interfaces;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace PROJECTNAME.Domain.Handlers
{
    public class RemoveENTITYNAMEHandler : IRequestHandler<RemoveENTITYNAMERequest, RemoveENTITYNAMEResponse>
    {
        private readonly ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> _repository;

        public RemoveENTITYNAMEHandler(ILiquidRepository<ENTITYNAME, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }

        ///<inheritdoc/>        
        public async Task<RemoveENTITYNAMEResponse> Handle(RemoveENTITYNAMERequest request, CancellationToken cancellationToken)
        {
            var data = await _repository.FindByIdAsync(request.Id);

            if (data != null)
            {
                await _repository.RemoveByIdAsync(request.Id);
                //await _mediator.Publish(new GenericEntityRemovedNotification<TEntity, TIdentifier>(entity));
            }

            return new RemoveENTITYNAMEResponse(data);
        }
    }
}
