using System;
using System.Threading.Tasks;
using Liquid.Core.Utils;
using Liquid.Core.Context;
using Liquid.WebApi.Http.Extensions;
using Microsoft.AspNetCore.Http;

namespace Liquid.WebApi.Http.Middlewares
{
    /// <summary>
    /// Gets the Context from request header and changes the context id.
    /// </summary>
    public class ContextDataHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ContextTag = "contextid";
        private readonly ILightContextFactory _contextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextDataHandlerMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="contextFactory">The context factory.</param>
        public ContextDataHandlerMiddleware(RequestDelegate next, ILightContextFactory contextFactory)
        {
            _next = next;
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Request handling method.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the current request.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            var contextGuid = context.GetHeaderValueFromRequest(ContextTag);
            if (!string.IsNullOrWhiteSpace(contextGuid) && contextGuid.IsGuid())
            {
                var lightContext = _contextFactory.GetContext();
                if (Guid.TryParse(contextGuid, out var contextId)) { lightContext.SetContextId(contextId); }
            }

            await _next(context);
        }
    }
}