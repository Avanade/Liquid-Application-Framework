using Castle.DynamicProxy;
using Liquid.Core.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Liquid.Core.Implementations
{
    /// <summary>
    /// Telemetry interceptor implementation.
    /// </summary>
    public class LiquidTelemetryInterceptor : LiquidInterceptorBase
    {
        private readonly ILogger<LiquidTelemetryInterceptor> _logger;
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// Initialize an instance of <see cref="LiquidTelemetryInterceptor"/>
        /// </summary>
        /// <param name="logger"></param>
        public LiquidTelemetryInterceptor(ILogger<LiquidTelemetryInterceptor> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
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
            _stopwatch.Stop();
            var elapsedTime = _stopwatch.Elapsed;

            _logger.LogInformation("Execution of {methodName} from {typeFullName} has ended in {milliseconds}ms.", invocation.Method.Name, invocation.TargetType.FullName, elapsedTime.TotalMilliseconds);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Generates log information from the start of method execution with metrics.
        /// </summary>
        /// <param name="invocation"> The method invocation.</param>
        /// <param name="proceedInfo"> The Castle.DynamicProxy.IInvocationProceedInfo.</param>
        protected override Task BeforeInvocation(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            _stopwatch.Start();
            _logger.LogInformation("Starting execution of {methodName} from {typeFullName}.", invocation.Method.Name, invocation.TargetType.FullName);
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
            _logger.LogError(exception, "Execution of {methodName} from {typeFullName} has thrown an exception.", invocation.Method.Name, invocation.TargetType.FullName);
            return Task.CompletedTask;
        }
    }
}
