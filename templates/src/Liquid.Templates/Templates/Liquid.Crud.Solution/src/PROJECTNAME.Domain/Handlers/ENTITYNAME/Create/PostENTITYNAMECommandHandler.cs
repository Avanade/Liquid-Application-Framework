using Liquid.Repository;
using MediatR;
using PROJECTNAME.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PROJECTNAME.Domain.Handlers.ENTITYNAME.Create
{
    public class PostENTITYNAMECommandHandler : IRequestHandler<PostENTITYNAMECommand>
    {
        private readonly ILiquidRepository<ENTITYNAMEEntity, int> _repository;

        public PostENTITYNAMECommandHandler(ILiquidRepository<ENTITYNAMEEntity, int> repository)
        {
            _repository = repository;
        }

        ///<inheritdoc/>        
        public async Task<Unit> Handle(PostENTITYNAMECommand request, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(request.Body);

            return new Unit();
        }
    }
}
