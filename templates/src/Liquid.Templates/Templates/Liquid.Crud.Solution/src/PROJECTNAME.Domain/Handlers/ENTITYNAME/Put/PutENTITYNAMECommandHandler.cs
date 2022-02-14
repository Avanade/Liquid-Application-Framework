using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Put
{
    public class PutENTITYNAMECommandHandler : IRequestHandler<PutENTITYNAMECommand, PutENTITYNAMECommandResponse>
    {
        private readonly ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> _repository;

        public PutENTITYNAMECommandHandler(ILiquidRepository<ENTITYNAMEEntity, ENTITYIDTYPE> repository)
        {
            _repository = repository;
        }


        public async Task<PutENTITYNAMECommandResponse> Handle(PutENTITYNAMECommand request, CancellationToken cancellationToken)
        {
            var data = await _repository.FindByIdAsync(request.Body.Id);

            if (data != null)
            {
                await _repository.UpdateAsync(request.Body);
            }

            return new PutENTITYNAMECommandResponse(request.Body);
        }
    }
}
