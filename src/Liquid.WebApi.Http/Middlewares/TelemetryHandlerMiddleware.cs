using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Liquid.Core.Configuration;
using Liquid.Core.Telemetry;
using Liquid.WebApi.Http.Configuration;
using Liquid.WebApi.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Liquid.WebApi.Http.Middlewares
{
    /// <summary>
    /// Executes the telemetry of the request and track all data executed on request.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class TelemetryHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly ILightConfiguration<ApiSettings> _apiSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="apiSettings">The API settings.</param>
        public TelemetryHandlerMiddleware(RequestDelegate next, ILightTelemetryFactory telemetryFactory, ILightConfiguration<ApiSettings> apiSettings)
        {
            _next = next;
            _telemetryFactory = telemetryFactory;
            _apiSettings = apiSettings;
        }

        /// <summary>
        /// Request handling method.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the current request.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public async Task InvokeAsync(HttpContext context)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            telemetry.AddContext("HttpRequest");
            if (_apiSettings?.Settings?.TrackRequests == true)
            {
                HttpRequest request = null;
                var requestBodyText = string.Empty;
                var stream = context.Response.Body;
                var responseBuffer = new MemoryStream();
                context.Response.Body = responseBuffer;

                try
                {
                    telemetry.StartTelemetryStopWatchMetric("RequestTracking");
                    request = context.Request;
                    requestBodyText = await request.GetRequestBody();
                    await _next(context);
                }
                finally
                {
                    try
                    {
                        var response = context.Response;
                        var responseBodyText = await response.GetResponseBody(stream, responseBuffer);
                        var trackingObject = new
                        {
                            httpRequest = new
                            {
                                method = request?.Method,
                                url = request?.GetDisplayUrl(),
                                headers = request?.Headers,
                                body = requestBodyText,
                                size = requestBodyText?.Length
                            },
                            httpResponse = new
                            {
                                statuscode = response?.StatusCode,
                                headers = response?.Headers,
                                body = responseBodyText,
                                size = responseBodyText?.Length
                            }
                        };

                        telemetry.CollectTelemetryStopWatchMetric("RequestTracking", trackingObject);
                        telemetry.RemoveContext("HttpRequest");
                        telemetry.Flush();
                    }
                    catch
                    {
                        // ignored. Left intentionally blank.
                    }
                }
            }
            else
            {
                try
                {
                    await _next(context);
                }
                finally
                {
                    telemetry.RemoveContext("HttpRequest");
                    telemetry.Flush();
                }
            }
        }
    }
}