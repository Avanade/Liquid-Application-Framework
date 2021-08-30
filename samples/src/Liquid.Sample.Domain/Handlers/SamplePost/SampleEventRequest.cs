using Liquid.Sample.Domain.Entities;
using MediatR;

namespace Liquid.Sample.Domain.Handlers
{
    public class SampleEventRequest : IRequest
    {
        public SampleMessageEntity Entity { get; set; }

        public SampleEventRequest(SampleMessageEntity entity)
        {
            Entity = entity;
        }
    }
}
