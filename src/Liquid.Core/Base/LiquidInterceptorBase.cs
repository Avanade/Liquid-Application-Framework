using Castle.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace Liquid.Core.Base
{
    /// <summary>
    /// Base class to implement interceptors with actions after, before and on exception.
    /// </summary>
    public abstract class LiquidInterceptorBase : AsyncInterceptorBase
    {
        /// <summary>
        /// Initialize an instace of <see cref="LiquidInterceptorBase"/>
        /// </summary>
        protected LiquidInterceptorBase()
        {
        }

        /// <summary>
        /// Traces start, stop and exception of the intercepted method.
        /// </summary>
        /// <param name="invocation"> The method invocation.</param>
        /// <param name="proceedInfo"> The Castle.DynamicProxy.IInvocationProceedInfo.</param>
        /// <param name="proceed">The function to proceed the proceedInfo.</param>
        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            try
            {
                await BeforeInvocation(invocation, proceedInfo);
                await proceed(invocation, proceedInfo).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await OnExceptionInvocation(invocation, proceedInfo, ex);
                throw;
            }
            finally
            {
                await AfterInvocation(invocation, proceedInfo, default(object));
            }
        }

        /// <summary>
        /// Traces start, stop and exception of the intercepted method.
        /// </summary>
        /// <param name="invocation"> The method invocation.</param>
        /// <param name="proceedInfo"> The Castle.DynamicProxy.IInvocationProceedInfo.</param>
        /// <param name="proceed">The function to proceed the proceedInfo.</param>
        protected sealed override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            TResult result = default;
            try
            {
                await BeforeInvocation(invocation, proceedInfo);
                return await proceed(invocation, proceedInfo).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await OnExceptionInvocation(invocation, proceedInfo, ex);
                throw;
            }
            finally
            {
                await AfterInvocation(invocation, proceedInfo, result).ConfigureAwait(false);
            }
        }

        /// <summary>
        ///   Override in derived classes to intercept method invocations.
        /// </summary>
        /// <typeparam name="TResult">The type of result object.</typeparam>
        /// <param name="invocation"> The method invocation.</param>
        /// <param name="proceedInfo"> The Castle.DynamicProxy.IInvocationProceedInfo.</param>
        /// <param name="result">Result object.</param>
        /// <returns></returns>
        protected abstract Task AfterInvocation<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, TResult result);

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <param name="invocation"> The method invocation.</param>
        /// <param name="proceedInfo"> The Castle.DynamicProxy.IInvocationProceedInfo.</param>
        /// <returns></returns>
        protected abstract Task BeforeInvocation(IInvocation invocation, IInvocationProceedInfo proceedInfo);

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <param name="invocation"> The method invocation.</param>
        /// <param name="proceedInfo"> The Castle.DynamicProxy.IInvocationProceedInfo.</param>
        /// <param name="exception">THe exception object.</param>
        /// <returns></returns>
        protected abstract Task OnExceptionInvocation(IInvocation invocation, IInvocationProceedInfo proceedInfo, Exception exception);
    }
}
