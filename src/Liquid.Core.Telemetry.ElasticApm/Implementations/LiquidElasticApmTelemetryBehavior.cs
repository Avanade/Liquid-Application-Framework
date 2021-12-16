using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Elastic.Apm.Api;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Liquid.Core.Telemetry.ElasticApm.MediatR
{
    /// <summary>
    /// Implements <see cref="IPipelineBehavior<TRequest, TResponse>"/> for Elastic APM.
    /// </summary>
    /// <typeparam name="TRequest">The type of request.</typeparam>
    /// <typeparam name="TResponse">Type of response object obtained upon return of request.</typeparam>
    public sealed class LiquidElasticApmTelemetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LiquidElasticApmTelemetryBehavior<TRequest, TResponse>> _logger;

        private readonly ITransaction _transaction;

        /// <summary>
        /// Initialize an instance of <see cref="LiquidPipelineBehavior<TRequest, TResponse>"/>
        /// </summary>
        /// <param name="logger"><see cref="ILogger<LoggingBehaviour<TRequest, TResponse>>"/> implementation.</param>
        /// <param name="tracer">Elastic APM <see cref="ITracer"/> implementation.</param>
        public LiquidElasticApmTelemetryBehavior(ILogger<LiquidElasticApmTelemetryBehavior<TRequest, TResponse>> logger, ITracer tracer)
        {
            _logger = logger;
            _transaction = tracer?.CurrentTransaction;
        }

        /// <summary>
        /// Handles MediatR pipeline operation.
        /// </summary>
        /// <param name="request">The request command or query.</param>
        /// <param name="cancellationToken">Notification about operation to be cancelled.</param>
        /// <param name="next">Mext operation to be performed.</param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var methodInfo = next.GetMethodInfo();

            TResponse response = default;

            var span = _transaction?.StartSpan(methodInfo.Name, methodInfo.ReflectedType.Name);
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
                span?.End();
            }

            return response;
        }

        private Task AfterResponse(MethodInfo methodInfo)
        {
            _logger?.LogInformation($"Execution of {methodInfo.Name} from {methodInfo.ReflectedType.Name} has ended.");
            return Task.CompletedTask;
        }

        private Task OnExceptionResponse(MethodInfo methodInfo, Exception exception)
        {
            _logger?.LogError(exception, $"Execution of {methodInfo.Name} from {methodInfo.ReflectedType.Name} has thrown an exception.");
            return Task.CompletedTask;
        }

        private Task BeforeRequest(MethodInfo methodInfo)
        {
            _logger?.LogInformation($"Starting execution of {methodInfo.Name} from {methodInfo.ReflectedType.Name}.");
            return Task.CompletedTask;
        }
    }
}
