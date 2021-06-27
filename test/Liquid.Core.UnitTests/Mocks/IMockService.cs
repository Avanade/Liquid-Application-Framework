using System.Threading.Tasks;

namespace Liquid.Core.UnitTests.Mocks
{
    public interface IMockService
    {
        Task<string> Get();
        Task<string> GetError();
    }
}