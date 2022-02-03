using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Update
{
    public class PutENTITYNAMECommandHandler : IRequestHandler<PutENTITYNAMECommand, PutENTITYNAMECommandResponse>
    {
        private readonly ILiquidRepository<Entities.ENTITYNAME, int> _repository;

        public PutENTITYNAMECommandHandler(ILiquidRepository<Entities.ENTITYNAME, int> repository)
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
