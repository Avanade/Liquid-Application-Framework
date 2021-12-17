using System.Threading.Tasks;

namespace Liquid.Core.Telemetry.ElasticApm.Tests.Mocks
{
    public interface IMockService
    {
        Task<string> Get();
        Task<string> GetError();
    }
}