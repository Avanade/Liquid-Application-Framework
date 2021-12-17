using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Elastic.Apm.Api;
using Liquid.Core.Telemetry.ElasticApm.Implementations;
using Liquid.Core.Telemetry.ElasticApm.Tests.Mocks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Liquid.Core.Telemetry.ElasticApm.Tests
{
    public class LiquidElasticApmInterceptorTests
    {
        private readonly LiquidElasticApmInterceptor _sut;

        private readonly IMockService _service;

        private readonly ILogger<LiquidElasticApmInterceptor> _logger;

        private readonly ITracer _tracer;

        public LiquidElasticApmInterceptorTests()
        {
            _logger = Substitute.For<ILogger<LiquidElasticApmInterceptor>>();

            _tracer = Substitute.For<ITracer>();

            _sut = new LiquidElasticApmInterceptor(_logger, _tracer);

            var generator = new ProxyGenerator();

            var logger = Substitute.For<ILogger<MockService>>();

            IMockService service = new MockService(logger);

            _service = generator.CreateInterfaceProxyWithTarget(service, _sut);
        }

        [Fact]
        public async Task Intercept_WhenMethodExecutionIsSucessfull_TracerStarts()
        {
            await _service.Get();

            _logger.Received(0);

            _tracer.Received(1);
        }

        [Fact]
        public async Task Intercept_WhenMethodExecutionThrowsException_TracerStartsLogsException()
        {
            await Assert.ThrowsAsync<NotImplementedException>(() => _service.GetError());

            _logger.Received(1);

            _tracer.Received(1);
        }
    }
}
