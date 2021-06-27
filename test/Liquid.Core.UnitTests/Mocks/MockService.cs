using Liquid.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Core.UnitTests.Mocks
{
    public class MockService : IMockService
    {
        private ILogger<MockService> _logger;
        public MockService(ILogger<MockService> logger)
        {
            _logger = logger;
        }
        public MockService()
        {

        }
        public async Task<string> Get()
        {
            _logger.LogInformation("sucess");
            return "Test";
        }

        public async Task<string> GetError()
        {
            throw new NotImplementedException();
        }
    }
}
