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
    /// Implements <see cref="IPipelineBehavior{TRequest, TResponse}"/> for Elastic APM.
    /// </summary>
    /// <typeparam name="TRequest">The type of request.</typeparam>
    /// <typeparam name="TResponse">Type of response object obtained upon return of request.</typeparam>
    public sealed class LiquidElasticApmTelemetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ITransaction _transaction;
        private ISpan _span;

        /// <summary>
        /// Initialize an instance of <see cref="LiquidElasticApmTelemetryBehavior{TRequest, TResponse}"/>
        /// </summary>
        /// <param name="tracer">Elastic APM <see cref="ITracer"/> implementation.</param>
        public LiquidElasticApmTelemetryBehavior(ITracer tracer)
        {
            _transaction = tracer?.CurrentTransaction;
        }

        /// <summary>
        /// Handles MediatR pipeline operation.
        /// </summary>
        /// <param name="request">The request command or query.</param>
        /// <param name="cancellationToken">Notification about operation to be cancelled.</param>
        /// <param name="next">Mext operation to be performed.</param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var methodInfo = next.GetMethodInfo();

            TResponse response = default;

            try
            {
                await BeforeRequest(methodInfo);
                response = await next();
            }
            finally
            {
                await AfterResponse(methodInfo);
            }

            return response;
        }

        private Task AfterResponse(MethodInfo methodInfo)
        {
            _span?.End();
            return Task.CompletedTask;
        }

        private Task BeforeRequest(MethodInfo methodInfo)
        {
            _span = _transaction?.StartSpan(methodInfo.Name, methodInfo.ReflectedType.Name);
            return Task.CompletedTask;
        }
    }
}
