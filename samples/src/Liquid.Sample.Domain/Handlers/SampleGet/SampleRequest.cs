using MediatR;
using System;

namespace Liquid.Sample.Domain.Handlers
{
    public class SampleRequest : IRequest<SampleResponse>
    {
        public string Id { get; set; }

        public SampleRequest(string id)
        {
            Id = id;
        }
    }
}
