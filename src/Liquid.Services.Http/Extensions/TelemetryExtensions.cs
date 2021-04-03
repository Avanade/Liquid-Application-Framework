using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Microsoft.Extensions.Logging;

namespace Liquid.Services.Http.Extensions
{
    /// <summary>
    /// Telemetry Extensions Class.
    /// </summary>
    internal static class TelemetryExtensions
    {

        /// <summary>
        /// Collects the HTTP call information asynchronous.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="key">The key.</param>
        /// <param name="responseMessage">The response message.</param>
        /// <param name="logger">The logger.</param>
        public static async Task CollectHttpCallInformationAsync(this ILightTelemetry telemetry, string key, HttpResponseMessage responseMessage, ILogger logger)
        {
            if (responseMessage != null)
            {
                var request = responseMessage.RequestMessage;
                var requestContent = request.Content;
                var requestBody = string.Empty;
                if (requestContent != null)
                {
                    requestBody = await requestContent.ReadAsStringAsync();
                }

                var responseContent = responseMessage.Content;
                var responseBody = string.Empty;
                if (responseContent != null)
                {
                    responseBody = await responseContent.ReadAsStringAsync();
                }

                var trackingObject = new
                {
                    title = "Liquid.Services.Http Log Information",
                    httpRequest = new
                    {
                        method = request.Method.Method,
                        url = request.RequestUri.ToString(),
                        headers = request.Headers.ToDictionary(),
                        size = requestBody.Length,
                        body = requestBody
                    },
                    httpResponse = new
                    {
                        statuscode = responseMessage.StatusCode,
                        headers = responseMessage.Headers.ToDictionary(),
                        size = responseBody.Length,
                        body = responseBody
                    }
                };

                if (!responseMessage.IsSuccessStatusCode && (HttpStatusCode.BadRequest.Equals(responseMessage.StatusCode) || HttpStatusCode.InternalServerError.Equals(responseMessage.StatusCode)))
                {
                    logger.LogError(trackingObject.ToJson());
                }

                telemetry.CollectTelemetryStopWatchMetric(key, trackingObject);
            }
        }
    }
}