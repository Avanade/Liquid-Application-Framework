using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Update
{
    public class UpdateENTITYNAMECommandHandler : IRequestHandler<UpdateENTITYNAMECommand, UpdateENTITYNAMECommandResponse>
    {
        private readonly ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> _repository;

        public UpdateENTITYNAMECommandHandler(ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }


        public async Task<UpdateENTITYNAMECommandResponse> Handle(UpdateENTITYNAMECommand request, CancellationToken cancellationToken)
        {
            var data = await _repository.FindByIdAsync(request.Body.Id);

            if (data != null)
            {
                await _repository.UpdateAsync(request.Body);
            }

            return new UpdateENTITYNAMECommandResponse(request.Body);
        }
    }
}
