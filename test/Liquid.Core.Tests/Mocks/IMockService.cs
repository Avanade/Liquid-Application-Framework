using System.Threading.Tasks;

namespace Liquid.Core.Tests.Mocks
{
    public interface IMockService
    {
        Task<string> Get();
        Task<string> GetError();
    }
}