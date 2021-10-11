using MediatR;

namespace Liquid.Sample.Domain.Handlers
{
    public class SampleRequest : IRequest<SampleResponse>
    {
        public int Id { get; set; }

        public SampleRequest(int id)
        {
            Id = id;
        }
    }
}
