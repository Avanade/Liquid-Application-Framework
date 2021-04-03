using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Liquid.Core.Configuration;
using Liquid.Core.Exceptions;
using Liquid.Core.Utils;
using Liquid.Core.Context;
using Liquid.WebApi.Http.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Liquid.WebApi.Http.Middlewares
{
    /// <summary>
    /// Global Exception Handler Middleware. 
    /// Responsible for handling all unexpected exceptions that may occur in each request.
    /// </summary>
    /// <seealso cref="ExceptionFilterAttribute" />
    [ExcludeFromCodeCoverage]
    internal sealed class ExceptionHandlerMiddleware : ExceptionFilterAttribute
    {
        private readonly RequestDelegate _next;
        private readonly ILightContextFactory _contextFactory;
        private readonly ILogger _logger;
        private readonly ILightConfiguration<ApiSettings> _apiConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware" /> class
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="apiConfiguration">The API configuration.</param>
        /// <exception cref="ArgumentNullException">next</exception>
        public ExceptionHandlerMiddleware(RequestDelegate next, ILightContextFactory contextFactory, ILoggerFactory loggerFactory, ILightConfiguration<ApiSettings> apiConfiguration)
        {
            _next = next;
            _contextFactory = contextFactory;
            _logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
            _apiConfiguration = apiConfiguration;
        }

        /// <summary>
        /// Request handling method.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the current request.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var exception = ex;
                if (ex is AggregateException aggregateException) exception = aggregateException.Flatten();
                var customContext = _contextFactory.GetContext();
                var correlation = customContext.ContextId.ToString();
                var timestamp = DateTime.Now;
                IDictionary<string, object> customData = new Dictionary<string, object>();
                if (exception is LightException)
                {
                    var customProperties = exception.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                    customProperties.Each(property => customData.Add($"customData_{property.Name}", property.GetValue(exception)));
                }
                var resultObject = new { timestamp, correlation, exception.Message, customData, innerException = exception.InnerException?.ToString(), exception.StackTrace }.ToJson(new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, });
                exception.Data.Add("correlation", correlation);
#pragma warning disable 4014
                Task.Run(() => _logger.LogError(exception, resultObject));
#pragma warning restore 4014

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error page middleware will not be executed.", Array.Empty<object>());
                    ExceptionDispatchInfo.Capture(exception).Throw();
                }
                try
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 500;
                    await DisplayRuntimeException(context, timestamp, correlation, exception, customData);
                    return;
                }
                catch (Exception ex1)
                {
                    _logger.LogError(0, ex1, "An exception was thrown attempting to display the error message.", Array.Empty<object>());
                }

                ExceptionDispatchInfo.Capture(exception).Throw();
            }
        }

        /// <summary>
        /// Displays the runtime exception.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="correlation">The correlation.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="customData">The custom data.</param>
        /// <returns></returns>
        private Task DisplayRuntimeException(HttpContext context, DateTime timestamp, string correlation, Exception exception, IDictionary<string, object> customData)
        {
            const string message = "An unhandled error has occurred, please enter in contact with technical team with the correlation code.";

            var showDetailedException = _apiConfiguration.Settings?.ShowDetailedException ?? false;

            var exceptionResultMessage = showDetailedException ? 
                new ExceptionResultResponseMessage
                {
                    Message = message, 
                    Timestamp = timestamp,
                    Correlation = correlation, 
                    ExceptionMessage = exception.Message, 
                    CustomData = customData, 
                    InnerExceptionText = exception.InnerException?.ToString(), 
                    StackTrace = exception.StackTrace
                } : 
                new ExceptionResultResponseMessage
                {
                    Message = message, 
                    Timestamp = timestamp, 
                    Correlation = correlation
                };

            var result = new ObjectResult(exceptionResultMessage);
            var routeData = context.GetRouteData();
            var actionDescriptor = new ActionDescriptor();
            var actionContext = new ActionContext(context, routeData, actionDescriptor);
            return result.ExecuteResultAsync(actionContext);
        }
    }

    /// <summary>
    /// Exception Result Response Message Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ExceptionResultResponseMessage
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the correlation.
        /// </summary>
        /// <value>
        /// The correlation.
        /// </value>
        [JsonProperty("correlation")]
        public string Correlation { get; set; }

        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        /// <value>
        /// The exception message.
        /// </value>
        [JsonProperty("exceptionMessage")]
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// Gets or sets the custom data.
        /// </summary>
        /// <value>
        /// The custom data.
        /// </value>
        [JsonProperty("customData")]
        public IDictionary<string, object> CustomData { get; set; }

        /// <summary>
        /// Gets or sets the inner exception text.
        /// </summary>
        /// <value>
        /// The inner exception text.
        /// </value>
        [JsonProperty("innerExceptionText")]
        public string InnerExceptionText { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>
        /// The stack trace.
        /// </value>
        [JsonProperty("stackTrace")]
        public string StackTrace { get; set; }
    }
}