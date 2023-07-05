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
        private readonly ITracer _tracer;

        public LiquidElasticApmTelemetryBehavior()
        {
            _tracer = Substitute.For<ITracer>();
        }

        [Fact]
        public async Task Handle_WhenMethodExecutionIsSucessfull_LogsStartEnd()
        {
            // Arrange
            var handler = Substitute.For<IRequestHandler<RequestMock, ResponseMock>>();
            var pipelineBehavior = new LiquidElasticApmTelemetryBehavior<RequestMock, ResponseMock>(_tracer); 

            // Act
            await pipelineBehavior.Handle(new RequestMock(), () => 
            { 
                return handler.Handle(new RequestMock(), CancellationToken.None); 
            }, CancellationToken.None);

            // Assert
            _tracer.Received(1);
        }

        [Fact]
        public async Task Handle_WhenMethodExecutionThrowsException_LogsException()
        {
            // Arrange
            var handler = new CommandHandlerMock();
            var pipelineBehavior = new LiquidElasticApmTelemetryBehavior<RequestMock, ResponseMock>(_tracer);

            // Act
            await Assert.ThrowsAsync<NotImplementedException>(() => 
                pipelineBehavior.Handle(new RequestMock(), () =>
                {
                    return handler.Handle(new RequestMock(), CancellationToken.None);
                }, CancellationToken.None)
            );

            // Assert
            _tracer.Received(1);
        }
    }
}
