using Liquid.Sample.Domain.Entities;
using System.Collections.Generic;

namespace Liquid.Sample.Domain.Handlers
{
    public class SampleResponse
    {
        public SampleEntity Response { get; set; }


        public SampleResponse(SampleEntity response)
        {
            Response = response;
        }
    }
}
