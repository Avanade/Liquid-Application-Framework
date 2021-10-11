using Liquid.Repository;
using Liquid.Sample.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Sample.Domain.Handlers.SamplePut
{
    public class PutCommandHandler : IRequestHandler<PutCommandRequest>
    {
        private readonly ILiquidRepository<SampleEntity, int> _repository;

        public PutCommandHandler(ILiquidRepository<SampleEntity, int> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(PutCommandRequest request, CancellationToken cancellationToken)
        {
            await _repository.UpdateAsync(new SampleEntity()
            {
                Id = request.Message.Id,
                MyProperty = request.Message.MyProperty
            });

            return new Unit();
        }
    }
}
