using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Remove
{
    public class RemoveENTITYNAMECommandHandler : IRequestHandler<RemoveENTITYNAMECommand, RemoveENTITYNAMECommandResponse>
    {
        private readonly ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> _repository;

        public RemoveENTITYNAMECommandHandler(ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }

        ///<inheritdoc/>        
        public async Task<RemoveENTITYNAMECommandResponse> Handle(RemoveENTITYNAMECommand request, CancellationToken cancellationToken)
        {
            var data = await _repository.FindByIdAsync(request.Id);

            if (data != null)
            {
                await _repository.RemoveByIdAsync(request.Id);
                //await _mediator.Publish(new GenericEntityRemovedNotification<TEntity, TIdentifier>(entity));
            }

            return new RemoveENTITYNAMECommandResponse(data);
        }
    }
}
