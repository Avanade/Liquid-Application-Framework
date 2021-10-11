using Liquid.Sample.Domain.Entities;
using MediatR;

namespace Liquid.Sample.Domain.Handlers.SamplePut
{
    public class PutCommandRequest : IRequest
    {
        public SampleMessageEntity Message { get; set; }

        public PutCommandRequest(SampleMessageEntity message)
        {
            Message = message;
        }
    }
}
