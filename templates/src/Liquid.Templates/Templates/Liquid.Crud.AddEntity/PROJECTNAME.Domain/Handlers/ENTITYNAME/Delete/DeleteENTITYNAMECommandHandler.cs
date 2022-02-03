using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Delete
{
    public class DeleteENTITYNAMECommandHandler : IRequestHandler<DeleteENTITYNAMECommand, DeleteENTITYNAMECommandResponse>
    {
        private readonly ILiquidRepository<Entities.ENTITYNAME, int> _repository;

        public DeleteENTITYNAMECommandHandler(ILiquidRepository<Entities.ENTITYNAME, int> repository)
        {
            _repository = repository;
        }

        ///<inheritdoc/>        
        public async Task<DeleteENTITYNAMECommandResponse> Handle(DeleteENTITYNAMECommand request, CancellationToken cancellationToken)
        {
            var data = await _repository.FindByIdAsync(request.Id);

            if (data != null)
            {
                await _repository.RemoveByIdAsync(request.Id);
                //await _mediator.Publish(new GenericEntityRemovedNotification<TEntity, TIdentifier>(entity));
            }

            return new DeleteENTITYNAMECommandResponse(data);
        }
    }
}
