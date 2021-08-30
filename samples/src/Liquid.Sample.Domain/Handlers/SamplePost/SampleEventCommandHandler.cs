using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Messaging;
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
        private readonly LiquidContext _context;

        public SampleEventCommandHandler(ILiquidProducer<SampleMessageEntity> producer, LiquidContext context)
        {
            _producer = producer;
            _context = context;
        }

        ///<inheritdoc/>        
        public async Task<Unit> Handle(SampleEventRequest request, CancellationToken cancellationToken)
        {
            await _producer.SendMessageAsync(request.Entity, _context.current);

            return new Unit();
        }
    }
}
