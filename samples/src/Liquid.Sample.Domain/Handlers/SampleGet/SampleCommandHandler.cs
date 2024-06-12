using Liquid.Repository;
using Liquid.Sample.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Sample.Domain.Handlers
{
    public class SampleCommandHandler : IRequestHandler<SampleRequest, SampleResponse>
    {
        private ILiquidRepository<SampleEntity, Guid> _repository;
        public SampleCommandHandler(ILiquidRepository<SampleEntity, Guid> repository)
        {
            _repository = repository;
        }

        ///<inheritdoc/>
        public async Task<SampleResponse> Handle(SampleRequest request, CancellationToken cancellationToken)
        {
            var item = await _repository.FindByIdAsync(Guid.Parse(request.Id));

            var result = new SampleResponse(item);

            return result;
        }
    }
}
