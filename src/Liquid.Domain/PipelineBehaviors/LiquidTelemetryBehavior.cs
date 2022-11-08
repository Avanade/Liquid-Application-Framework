using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Domain.PipelineBehaviors
{
    /// <summary>
    /// Telemetry Behavior implementation for Mediator pipelines.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class LiquidTelemetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LiquidTelemetryBehavior<TRequest, TResponse>> _logger;
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// Initialize an instance of <see cref="LiquidTelemetryBehavior{TRequest, TResponse}"/>
        /// </summary>
        /// <param name="logger">Logging object.</param>
        public LiquidTelemetryBehavior(ILogger<LiquidTelemetryBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        /// <summary>
        /// Pipeline handler. Generates telemetry logs before, after and on exception case during request execution.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="next"> Awaitable delegate for the next action in the pipeline. Eventually this delegate 
        /// represents the handler.</param>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var methodInfo = next.GetMethodInfo();

            TResponse response = default;

            try
            {
                await BeforeRequest(methodInfo);
                response = await next();
            }
            catch (Exception ex)
            {
                await OnExceptionResponse(methodInfo, ex);
                throw;
            }
            finally
            {
                await AfterResponse(methodInfo);
            }

            return response;
        }

        private Task AfterResponse(MethodInfo methodInfo)
        {
            _stopwatch.Stop();
            var elapsedTime = _stopwatch.Elapsed;

            _logger.LogInformation("Execution of {methodName} from {typeFullName} has ended in {milliseconds}ms."
                , methodInfo.Name, nameof(methodInfo.ReflectedType), elapsedTime.TotalMilliseconds);

            return Task.CompletedTask;
        }

        private Task OnExceptionResponse(MethodInfo methodInfo, Exception exception)
        {
            _logger.LogError(exception, "Execution of {methodName} from {typeFullName} has thrown an exception.", methodInfo.Name, nameof(methodInfo.ReflectedType));
            return Task.CompletedTask;
        }

        private Task BeforeRequest(MethodInfo methodInfo)
        {
            _stopwatch.Start();
            _logger.LogInformation("Starting execution of {methodName} from {typeFullName}.", methodInfo.Name, methodInfo.ReflectedType);
            return Task.CompletedTask;
        }
    }
}
