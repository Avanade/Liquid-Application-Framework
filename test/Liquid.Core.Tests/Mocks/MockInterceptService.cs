using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Liquid.Core.UnitTests.Mocks
{
    public class MockInterceptService : IMockService
    {
        
        public MockInterceptService()
        {

        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> Get()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return "Test";
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> GetError()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }
    }
}
