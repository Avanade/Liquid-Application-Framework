using Liquid.Repository;
using Liquid.Sample.Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Sample.Domain.Handlers
{
    public class SampleCommandHandler : IRequestHandler<SampleRequest, SampleResponse>
    {
        private ILiquidRepository<SampleEntity, int> _repository;
        public SampleCommandHandler(ILiquidRepository<SampleEntity, int> repository)
        {
            _repository = repository;
        }

        ///<inheritdoc/>
        public async Task<SampleResponse> Handle(SampleRequest request, CancellationToken cancellationToken)
        {
            var item = await _repository.FindByIdAsync(request.Id);

            var result = new SampleResponse(item);

            return result;
        }
    }
}
