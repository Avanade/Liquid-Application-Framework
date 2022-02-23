using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Delete
{
    public class DeleteENTITYNAMECommandHandler : IRequestHandler<DeleteENTITYNAMECommand, DeleteENTITYNAMECommandResponse>
    {
        private readonly ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> _repository;

        public DeleteENTITYNAMECommandHandler(ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> repository)
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
            }

            return new DeleteENTITYNAMECommandResponse(data);
        }
    }
}
