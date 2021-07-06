using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Liquid.WebApi.Http.Middlewares
{
    /// <summary>
    /// Generates a log and serialize a formated Json response object for generic exceptions.
    /// Includes its behavior in netcore pipelines after request execution when thows error.
    /// </summary>
    public class LiquidExceptionMiddleware
    {
        private readonly ILogger<LiquidExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidExceptionMiddleware"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="next"></param>
        public LiquidExceptionMiddleware(ILogger<LiquidExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// Generates a log and serialize a formated Json response object for generic exceptions.
        /// Includes its behavior in netcore pipelines after request execution when thows error.
        /// </summary>
        /// <param name="context">HTTP-specific information about an individual HTTP request.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var json = new
            {
                context.Response.StatusCode,
                Message = "An error occurred whilst processing your request",
                Detailed = exception
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(json));
        }
    }
}
