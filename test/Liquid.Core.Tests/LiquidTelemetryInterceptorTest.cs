using Castle.DynamicProxy;
using Liquid.Core.Implementations;
using Liquid.Core.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Core.UnitTests
{
    public class LiquidTelemetryInterceptorTest
    {
        private LiquidTelemetryInterceptor _sut;
        private IMockService _input;
        private ILogger<LiquidTelemetryInterceptor> _logger;

        public LiquidTelemetryInterceptorTest()
        {
            var generator = new ProxyGenerator();

            var logger = Substitute.For<ILogger<MockService>>();

            IMockService service = new MockService(logger);

            _logger = Substitute.For<ILogger<LiquidTelemetryInterceptor>>();

            _sut = new LiquidTelemetryInterceptor(_logger);

            _input = generator.CreateInterfaceProxyWithTarget(service, _sut);

        }

        [Fact]
        public async Task Intercept_WhenMethodExecutionIsSucessfull_LogStarEnd()
        {
            await _input.Get();

            _logger.Received(2);

        }

        [Fact]
        public async Task Intercept_WhenMethodExecutionThrowsException_LogStarExceptionAndEnd()
        {
            await Assert.ThrowsAsync<NotImplementedException>(() => _input.GetError());

            _logger.Received(3);
        }
    }
}
