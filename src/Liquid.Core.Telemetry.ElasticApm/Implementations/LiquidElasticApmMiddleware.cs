using Elastic.Apm.Api;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Liquid.Core.Telemetry.ElasticApm.Implementations
{
    /// <summary>
    /// Implement a middleware for Elastic APM adding the trace id to the response header.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LiquidElasticApmMiddleware
    {

        private readonly ITracer _tracer;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initialize an instance of <see cref="LiquidElasticApmInterceptor"/>.
        /// </summary>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        /// <param name="tracer">Elastic APM <see cref="ITracer"/> implementation.</param>
        public LiquidElasticApmMiddleware(RequestDelegate next, ITracer tracer)
        {
            _tracer = tracer;
            _next = next;
        }

        /// <summary>
        /// Request handling method, adding the custom header to the response.
        /// </summary>
        /// <param name="context">The Microsoft.AspNetCore.Http.HttpContext for the current request.</param>
        /// <returns>A System.Threading.Tasks.Task that represents the execution of this middleware.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("trace-id", _tracer.CurrentTransaction.Id);
            await _next(context);
        }

    }
}
