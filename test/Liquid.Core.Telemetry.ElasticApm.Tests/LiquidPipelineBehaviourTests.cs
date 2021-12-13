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
    public class LiquidPipelineBehaviorTests
    {
        private readonly ILogger<LiquidPipelineBehavior<RequestMock, ResponseMock>> _logger;

        private readonly ITracer _tracer;

        public LiquidPipelineBehaviorTests()
        {
            _logger = Substitute.For<ILogger<LiquidPipelineBehavior<RequestMock, ResponseMock>>>();
            _tracer = Substitute.For<ITracer>();
        }

        [Fact]
        public async Task Handle_WhenMethodExecutionIsSucessfull_LogsStartEnd()
        {
            // Arrange
            var handler = Substitute.For<IRequestHandler<RequestMock, ResponseMock>>();
            var pipelineBehavior = new LiquidPipelineBehavior<RequestMock, ResponseMock>(_logger, _tracer); 

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
            var pipelineBehavior = new LiquidPipelineBehavior<RequestMock, ResponseMock>(_logger, _tracer);

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
