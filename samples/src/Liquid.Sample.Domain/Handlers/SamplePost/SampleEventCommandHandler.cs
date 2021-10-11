using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Messaging;
using Liquid.Messaging.Interfaces;
using Liquid.Repository;
using Liquid.Sample.Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Sample.Domain.Handlers
{
    public class SampleEventCommandHandler : IRequestHandler<SampleEventRequest>
    {
        private ILiquidProducer<SampleMessageEntity> _producer;
        private readonly ILiquidRepository<SampleEntity, int> _repository;
        private readonly ILiquidContext _context;

        public SampleEventCommandHandler(ILiquidProducer<SampleMessageEntity> producer, ILiquidContext context, ILiquidRepository<SampleEntity, int> repository)
        {
            _producer = producer;
            _context = context;
            _repository = repository;
        }

        ///<inheritdoc/>        
        public async Task<Unit> Handle(SampleEventRequest request, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(new SampleEntity()
            {
                Id = request.Entity.Id,
                MyProperty = request.Entity.MyProperty
            });

            await _producer.SendMessageAsync(request.Entity, _context.current);

            return new Unit();
        }
    }
}
