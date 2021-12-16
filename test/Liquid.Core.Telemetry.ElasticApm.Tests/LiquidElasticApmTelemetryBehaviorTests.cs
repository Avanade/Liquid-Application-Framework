using System;
using System.Threading;
using System.Threading.Tasks;
using Elastic.Apm.Api;
using Liquid.Core.Telemetry.ElasticApm.MediatR;
using Liquid.Core.Telemetry.ElasticApm.Tests.Mocks;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Liquid.Core.Telemetry.ElasticApm.Tests
{
    public class LiquidElasticApmTelemetryBehavior
    {
        private readonly ILogger<LiquidElasticApmTelemetryBehavior<RequestMock, ResponseMock>> _logger;

        private readonly ITracer _tracer;

        public LiquidElasticApmTelemetryBehavior()
        {
            _logger = Substitute.For<ILogger<LiquidElasticApmTelemetryBehavior<RequestMock, ResponseMock>>>();
            _tracer = Substitute.For<ITracer>();
        }

        [Fact]
        public async Task Handle_WhenMethodExecutionIsSucessfull_LogsStartEnd()
        {
            // Arrange
            var handler = Substitute.For<IRequestHandler<RequestMock, ResponseMock>>();
            var pipelineBehavior = new LiquidElasticApmTelemetryBehavior<RequestMock, ResponseMock>(_logger, _tracer); 

            // Act
            await pipelineBehavior.Handle(new RequestMock(), CancellationToken.None, () => 
            { 
                return handler.Handle(new RequestMock(), CancellationToken.None); 
            });

            // Assert
            _logger.Received(2);
            _tracer.Received(1);
        }

        [Fact]
        public async Task Handle_WhenMethodExecutionThrowsException_LogsException()
        {
            // Arrange
            var handler = new CommandHandlerMock();
            var pipelineBehavior = new LiquidElasticApmTelemetryBehavior<RequestMock, ResponseMock>(_logger, _tracer);

            // Act
            await Assert.ThrowsAsync<NotImplementedException>(() => 
                pipelineBehavior.Handle(new RequestMock(), CancellationToken.None, () =>
                {
                    return handler.Handle(new RequestMock(), CancellationToken.None);
                })
            );

            // Assert
            _logger.Received(3);
            _tracer.Received(1);
        }
    }
}
