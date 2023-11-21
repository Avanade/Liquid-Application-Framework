using Liquid.Adapter.Dataverse;
using Liquid.Adapter.Dataverse.Extensions;
using Microsoft.Xrm.Sdk;

namespace Liquid.Sample.Dataverse.Domain
{
    public class PostSampleService
    {
        private readonly ILiquidDataverseAdapter _adapter;

        private readonly ILiquidMapper<string,Entity> _mapper;

        public PostSampleService(ILiquidDataverseAdapter adapter, ILiquidMapper<string, Entity> mapper)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<string> PostSample(string body)
        {
            var bodyEntity = await _mapper.Map(body, "sample");

            var result = await _adapter.Create(bodyEntity);

            bodyEntity.Id = result;

            var response = bodyEntity.ToJsonString();

            return response;
        }
    }
}