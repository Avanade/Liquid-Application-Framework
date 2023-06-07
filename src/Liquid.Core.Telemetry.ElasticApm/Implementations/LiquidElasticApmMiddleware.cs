using Elastic.Apm.Api;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Core.Telemetry.ElasticApm.Implementations
{
    /// <summary>
    /// Implement a middleware for Elastic APM adding the trace id to the response header.
    /// </summary>
    public class LiquidElasticApmMiddleware : IMiddleware
    {

        private readonly ITracer _tracer;

        /// <summary>
        /// Initialize an instance of <see cref="LiquidElasticApmInterceptor"/>.
        /// </summary>
        /// <param name="tracer">Elastic APM <see cref="ITracer"/> implementation.</param>
        public LiquidElasticApmMiddleware(ITracer tracer)
        {
            _tracer = tracer;
        }

        /// <summary>
        /// Request handling method, adding the custom header to the response.
        /// </summary>
        /// <param name="context">The Microsoft.AspNetCore.Http.HttpContext for the current request.</param>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        /// <returns>A System.Threading.Tasks.Task that represents the execution of this middleware.</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Add("Elastic-Apm-Trace-Id", _tracer.CurrentTransaction.Id);
            await next.Invoke(context);
        }

    }
}
