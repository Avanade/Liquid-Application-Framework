using Liquid.Core.Interfaces;
using Liquid.WebApi.Http.Exceptions;
using Liquid.WebApi.Http.Extensions;
using Liquid.WebApi.Http.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.Middlewares
{
    /// <summary>
    /// Inserts configured context keys in ILogger service scope.
    /// Includes its behavior in netcore pipelines before request execution.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LiquidScopedLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LiquidScopedLoggingMiddleware> _logger;
        private readonly ILiquidConfiguration<ScopedLoggingSettings> _options;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidScopedLoggingMiddleware"/>
        /// </summary>
        /// <param name="next">Invoked request.</param>
        /// <param name="logger">Logger service instance.</param>
        /// <param name="options">Context keys set.</param>
        public LiquidScopedLoggingMiddleware(RequestDelegate next, ILogger<LiquidScopedLoggingMiddleware> logger, ILiquidConfiguration<ScopedLoggingSettings> options)
        {
            _next = next;
            _logger = logger;
            _options = options;
        }

        /// <summary>
        /// Obtains the value of configured keys from HttpContext and inserts them in ILogger service scope.
        /// Includes its behavior in netcore pipelines before request execution.
        /// </summary>
        /// <param name="context">HTTP-specific information about an individual HTTP request.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            var scope = new List<KeyValuePair<string, object>>();            

            foreach (var key in _options.Settings.Keys)
            {
                var value = context.GetValueFromHeader(key.KeyName);

                if (string.IsNullOrEmpty(value))
                    value = context.GetValueFromQuery(key.KeyName);

                if (string.IsNullOrEmpty(value) && key.Required)
                    throw new LiquidScopedtKeysException(key.KeyName);

                scope.Add(new KeyValuePair<string, object>(key.KeyName, value));
            }

            using (_logger.BeginScope(scope.ToArray()))
            {
                await _next(context);
            }
        }
    }
}
