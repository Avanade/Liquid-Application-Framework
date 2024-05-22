﻿using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Liquid.WebApi.Http.Exceptions;
using Liquid.WebApi.Http.Extensions;
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
    /// <remarks>
    /// Initialize a new instance of <see cref="LiquidScopedLoggingMiddleware"/>
    /// </remarks>
    /// <param name="next">Invoked request.</param>
    /// <param name="logger">Logger service instance.</param>
    /// <param name="options">Context keys set.</param>
    [ExcludeFromCodeCoverage]
    public class LiquidScopedLoggingMiddleware(RequestDelegate next, ILogger<LiquidScopedLoggingMiddleware> logger, ILiquidConfiguration<ScopedLoggingSettings> options)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<LiquidScopedLoggingMiddleware> _logger = logger;
        private readonly ILiquidConfiguration<ScopedLoggingSettings> _options = options;

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
