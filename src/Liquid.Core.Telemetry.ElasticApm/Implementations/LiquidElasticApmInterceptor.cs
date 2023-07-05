using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Elastic.Apm.Api;
using Liquid.Core.Base;
using Microsoft.Extensions.Logging;

namespace Liquid.Core.Telemetry.ElasticApm.Implementations
{
    /// <summary>
    /// Implement interceptors for Elastic APM with actions after, before and on exception.
    /// </summary>
    public sealed class LiquidElasticApmInterceptor : LiquidInterceptorBase
    {
        private readonly ILogger<LiquidElasticApmInterceptor> _logger;

        private readonly ITracer _tracer;

        private ISpan _span;

        /// <summary>
        /// Initialize an instance of <see cref="LiquidElasticApmInterceptor"/>
        /// </summary>
        /// <param name="tracer">Elastic APM <see cref="ITracer"/> implementation.</param>
        public LiquidElasticApmInterceptor(ITracer tracer)
        {
            _tracer = tracer;
        }

        /// <summary>
        /// Generates log information from the end of method execution with metrics.
        /// </summary>
        /// <typeparam name="TResult">Type of results object.</typeparam>
        /// <param name="invocation"> The method invocation.</param>
        /// <param name="proceedInfo"> The Castle.DynamicProxy.IInvocationProceedInfo.</param>
        /// <param name="result">Result object.</param>
        protected override Task AfterInvocation<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, TResult result)
        {
            _span?.End();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Generates log information from the start of method execution with metrics.
        /// </summary>
        /// <param name="invocation"> The method invocation.</param>
        /// <param name="proceedInfo"> The Castle.DynamicProxy.IInvocationProceedInfo.</param>
        protected override Task BeforeInvocation(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            _span = _tracer?.CurrentTransaction?.StartSpan($"{invocation.TargetType.Name}.{invocation.Method.Name}", invocation.TargetType.Name);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Generates an error log of the exception thrown by the method.
        /// </summary>
        /// <param name="invocation"> The method invocation.</param>
        /// <param name="proceedInfo"> The Castle.DynamicProxy.IInvocationProceedInfo.</param>
        /// <param name="exception">The <see cref="Exception"/> object.</param>
        protected override Task OnExceptionInvocation(IInvocation invocation, IInvocationProceedInfo proceedInfo, Exception exception)
        {
            _span.Outcome = Outcome.Failure;
            return Task.CompletedTask;
        }
    }
}
