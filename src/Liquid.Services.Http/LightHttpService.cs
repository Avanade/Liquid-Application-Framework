using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Liquid.Services.Configuration;
using Liquid.Services.Http.ContentTypes;
using Liquid.Services.Http.Entities;
using Liquid.Services.Http.Enum;
using Liquid.Services.Http.Exceptions;
using Liquid.Services.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace Liquid.Services.Http
{
    /// <summary>
    /// Http service client using System.Net.HttpClient
    /// </summary>
    /// <seealso cref="Liquid.Services.LightService" />
    /// <seealso cref="Liquid.Services.Http.ILightHttpService" />
    public class LightHttpService : LightService, ILightHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightHttpService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="servicesSettings">The services settings.</param>
        /// <param name="mapperService">The mapper service.</param>
        public LightHttpService(IHttpClientFactory httpClientFactory,
                                ILoggerFactory loggerFactory,
                                ILightContextFactory contextFactory,
                                ILightTelemetryFactory telemetryFactory,
                                ILightServiceConfiguration<LightServiceSetting> servicesSettings,
                                IMapper mapperService) : base(loggerFactory, contextFactory, telemetryFactory, servicesSettings, mapperService)
        {
            _httpClientFactory = httpClientFactory;
        }


        /// <summary>
        /// Executes the Get request against an endpoint url.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        public async Task<HttpServiceResponse<TResponse>> GetAsync<TResponse>(string endpoint, Dictionary<string, string> customHeaders = null)
        {
            return await SendRequestAsync<Type, TResponse>(endpoint, HttpMethod.Get, customHeaders: customHeaders);
        }

        /// <summary>
        /// Posts the request against an endpoint url.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="request">The request object.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        public async Task<HttpServiceResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest request = default, Dictionary<string, string> customHeaders = null, ContentTypeFormat contentType = ContentTypeFormat.Json)
        {
            return await SendRequestAsync<TRequest, TResponse>(endpoint, HttpMethod.Post, request, customHeaders, contentType);
        }

        /// <summary>
        /// Changes the resource in the specified url endpoint.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="request">The request object.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        public async Task<HttpServiceResponse<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest request = default, Dictionary<string, string> customHeaders = null, ContentTypeFormat contentType = ContentTypeFormat.Json)
        {
            return await SendRequestAsync<TRequest, TResponse>(endpoint, HttpMethod.Put, request, customHeaders, contentType);
        }

        /// <summary>
        /// Deletes the specified resource in the url endpoint.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        /// <exception cref="HttpServiceCallException">PUT</exception>
        public async Task<HttpServiceResponse<TResponse>> DeleteAsync<TResponse>(string endpoint, Dictionary<string, string> customHeaders = null)
        {
            return await SendRequestAsync<Type, TResponse>(endpoint, HttpMethod.Delete, null, customHeaders);
        }

        /// <summary>
        /// Deletes the specified resource in the url endpoint.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="request">The request.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        /// <exception cref="HttpServiceCallException">PUT</exception>
        public async Task<HttpServiceResponse<TResponse>> DeleteAsync<TRequest, TResponse>(string endpoint, TRequest request = default, Dictionary<string, string> customHeaders = null, ContentTypeFormat contentType = ContentTypeFormat.Json)
        {
            return await SendRequestAsync<TRequest, TResponse>(endpoint, HttpMethod.Delete, request, customHeaders, contentType);
        }

        /// <summary>
        /// Sends a custom request to an url endpoint. The request method could be any type (GET, POST, PUT, DELETE, OPTIONS, etc...)
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="method">The Http method (GET, POST, PUT, DELETE, OPTIONS, etc...).</param>
        /// <param name="request">The request object.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        public async Task<HttpServiceResponse<TResponse>> SendRequestAsync<TRequest, TResponse>(string endpoint, HttpMethod method, TRequest request = default, Dictionary<string, string> customHeaders = null, ContentTypeFormat contentType = ContentTypeFormat.Json)
        {
            var url = $"{ServiceSettings.Address?.TrimEnd('/')}/{endpoint?.TrimStart('/')}";
            var telemetry = TelemetryFactory.GetTelemetry();
            HttpContent requestContent = null;
            try
            {
                telemetry.AddContext("HttpServiceRequest");

                var telemetryMetricKey = $"HttpCall_{method}_{url}";
                telemetry.StartTelemetryStopWatchMetric(telemetryMetricKey);

                HttpResponseMessage httpResponse = null;
                await Resilience.HandleAsync(async () =>
                {
                    var client = _httpClientFactory.CreateClient(ServiceId);
                    client.Timeout = TimeSpan.FromSeconds(ServiceSettings.Timeout);
                    var requestMessage = new HttpRequestMessage(method, url);
                    SetCustomHeaders(customHeaders, requestMessage.Headers);
                    if (request != null)
                    {
                        requestContent = request.GenerateContent(contentType);
                        requestMessage.Content = requestContent;
                    }
                    httpResponse = await client.SendAsync(requestMessage);
                    await telemetry.CollectHttpCallInformationAsync(telemetryMetricKey, httpResponse, Logger);
                });

                return httpResponse != null ? new HttpServiceResponse<TResponse>(httpResponse) : null;
            }
            catch (Exception ex)
            {
                var lex = new HttpServiceCallException(ex, url, method.ToString().ToUpper(), requestContent?.ReadAsStringAsync().Result);
                Logger.LogError(lex, HttpServiceCallException.DefaultExceptionMessage);
                throw lex;
            }
            finally
            {
                telemetry.RemoveContext("HttpServiceRequest");
            }
        }

        /// <summary>
        /// Sends a custom stream request to an url endpoint. The request method could be any type (GET, POST, PUT, DELETE, OPTIONS, etc...)
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="method">The Http method (GET, POST, PUT, DELETE, OPTIONS, etc...).</param>
        /// <param name="stream">The content stream to be sent.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        public async Task<HttpServiceResponse<TResponse>> SendStreamRequestAsync<TResponse>(string endpoint, HttpMethod method, Stream stream = null, Dictionary<string, string> customHeaders = null)
        {
            var url = $"{ServiceSettings.Address?.TrimEnd('/')}/{endpoint?.TrimStart('/')}";
            var telemetry = TelemetryFactory.GetTelemetry();
            HttpContent requestContent = null;
            try
            {
                telemetry.AddContext("HttpServiceRequest");

                var telemetryMetricKey = $"HttpCall_{method}_{url}";
                telemetry.StartTelemetryStopWatchMetric(telemetryMetricKey);

                HttpResponseMessage httpResponse = null;
                await Resilience.HandleAsync(async () =>
                {
                    var client = _httpClientFactory.CreateClient(ServiceId);
                    client.Timeout = TimeSpan.FromSeconds(ServiceSettings.Timeout);
                    var requestMessage = new HttpRequestMessage(method, url);
                    SetCustomHeaders(customHeaders, requestMessage.Headers);
                    if (stream != null)
                    {
                        requestContent = new MultipartFormDataContent { new StreamContent(stream) };
                        requestMessage.Content = requestContent;
                    }
                    httpResponse = await client.SendAsync(requestMessage);
                    await telemetry.CollectHttpCallInformationAsync(telemetryMetricKey, httpResponse, Logger);
                });

                return httpResponse != null ? new HttpServiceResponse<TResponse>(httpResponse) : null;
            }
            catch (Exception ex)
            {
                var lex = new HttpServiceCallException(ex, url, method.ToString().ToUpper(), requestContent?.ReadAsStringAsync().Result);
                Logger.LogError(lex, HttpServiceCallException.DefaultExceptionMessage);
                throw lex;
            }
            finally
            {
                telemetry.RemoveContext("HttpServiceRequest");
            }
        }

        /// <summary>
        /// Executes a GraphQL GET request against an endpoint url.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="query">The command query.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// endpoint
        /// or
        /// query
        /// </exception>
        /// <exception cref="System.ArgumentNullException">endpoint
        /// or
        /// query</exception>
        public async Task<GraphQlServiceResponse<TResponse>> GraphQlGetAsync<TResponse>(string endpoint, string query, Dictionary<string, string> customHeaders = null)
        {
            return await GraphQlGetAsync<TResponse>(endpoint, new GraphQlRequest { Query = query }, customHeaders);
        }

        /// <summary>
        /// Executes a GraphQL GET request against an endpoint url.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="request">The GraphQL request query.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// endpoint
        /// or
        /// request
        /// or
        /// Query
        /// </exception>
        public async Task<GraphQlServiceResponse<TResponse>> GraphQlGetAsync<TResponse>(string endpoint, GraphQlRequest request, Dictionary<string, string> customHeaders = null)
        {
            if (endpoint == null) { throw new ArgumentNullException(nameof(endpoint)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Query == null) { throw new ArgumentNullException(nameof(request.Query)); }

            var url = $"{ServiceSettings.Address?.TrimEnd('/')}/{endpoint.TrimStart('/')}";
            var telemetry = TelemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("GraphQlServiceRequest");

                var telemetryMetricKey = $"GraphCall_{HttpMethod.Get}_{url}";
                telemetry.StartTelemetryStopWatchMetric(telemetryMetricKey);

                var queryParamsBuilder = new StringBuilder($"query={request.Query}");
                if (request.Variables != null) { queryParamsBuilder.Append($"&variables={request.Variables.ToJson()}"); }
                HttpResponseMessage httpResponse = null;
                await Resilience.HandleAsync(async () =>
                {
                    var client = _httpClientFactory.CreateClient(ServiceId);
                    client.Timeout = TimeSpan.FromSeconds(ServiceSettings.Timeout);
                    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{url}/?{queryParamsBuilder.ToString().RemoveLineEndings()}");
                    SetCustomHeaders(customHeaders, httpRequestMessage.Headers);
                    httpResponse = await client.SendAsync(httpRequestMessage);
                    await telemetry.CollectHttpCallInformationAsync(telemetryMetricKey, httpResponse, Logger);
                });

                return httpResponse != null ? new GraphQlServiceResponse<TResponse>(httpResponse) : null;
            }
            catch (Exception ex)
            {
                var lex = new HttpServiceCallException(ex, url, HttpMethod.Get.ToString());
                Logger.LogError(lex, HttpServiceCallException.DefaultExceptionMessage);
                throw lex;
            }
            finally
            {
                telemetry.RemoveContext("GraphQlServiceRequest");
            }
        }

        /// <summary>
        /// Executes a GraphQL POST request against an endpoint url.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="commandQuery">The command query.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// endpoint
        /// or
        /// query
        /// </exception>
        /// <exception cref="System.ArgumentNullException">endpoint
        /// or
        /// query</exception>
        public async Task<GraphQlServiceResponse<TResponse>> GraphQlPostAsync<TResponse>(string endpoint, string commandQuery, Dictionary<string, string> customHeaders = null)
        {
            return await GraphQlPostAsync<TResponse>(endpoint, new GraphQlRequest { Query = commandQuery }, customHeaders);
        }

        /// <summary>
        /// Executes a GraphQL POST request against an endpoint url.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="request">The GraphQL request query.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// endpoint
        /// or
        /// request
        /// or
        /// Query
        /// </exception>
        public async Task<GraphQlServiceResponse<TResponse>> GraphQlPostAsync<TResponse>(string endpoint, GraphQlRequest request, Dictionary<string, string> customHeaders = null)
        {
            if (endpoint == null) { throw new ArgumentNullException(nameof(endpoint)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Query == null) { throw new ArgumentNullException(nameof(request.Query)); }

            HttpContent requestContent = null;
            var url = $"{ServiceSettings.Address?.TrimEnd('/')}/{endpoint.TrimStart('/')}";
            var telemetry = TelemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("GraphQlServiceRequest");

                var telemetryMetricKey = $"GraphCall_{HttpMethod.Post}_{url}";
                telemetry.StartTelemetryStopWatchMetric(telemetryMetricKey);

                request.Variables = request.Variables.ToJson();
                var query = request.ToJson();
                requestContent = new StringContent(query, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse = null;
                await Resilience.HandleAsync(async () =>
                {
                    var client = _httpClientFactory.CreateClient(ServiceId);
                    client.Timeout = TimeSpan.FromSeconds(ServiceSettings.Timeout);
                    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url) { Content = requestContent };
                    SetCustomHeaders(customHeaders, httpRequestMessage.Headers);
                    httpResponse = await client.SendAsync(httpRequestMessage);
                    await telemetry.CollectHttpCallInformationAsync(telemetryMetricKey, httpResponse, Logger);
                });

                return httpResponse != null ? new GraphQlServiceResponse<TResponse>(httpResponse) : null;
            }
            catch (Exception ex)
            {
                var lex = new HttpServiceCallException(ex, url, HttpMethod.Post.ToString(), requestContent?.ReadAsStringAsync().Result);
                Logger.LogError(lex, HttpServiceCallException.DefaultExceptionMessage);
                throw lex;
            }
            finally
            {
                telemetry.RemoveContext("GraphQlServiceRequest");
            }
        }

        /// <summary>Sets the custom headers of http request.</summary>
        /// <param name="customHeaders">The custom headers.</param>
        /// <param name="headers">The headers.</param>
        private void SetCustomHeaders(Dictionary<string, string> customHeaders, HttpHeaders headers)
        {
            headers.Clear();
            var context = ContextFactory.GetContext();
            if (context != null)
            {
                headers.TryAddWithoutValidation("liguid_cid", context.ContextId.ToString());
                headers.TryAddWithoutValidation("liquid_bcid", context.BusinessContextId.ToString());
                headers.TryAddWithoutValidation("culture", context.ContextCulture);
                headers.TryAddWithoutValidation("channel", context.ContextChannel);
            }

            if (customHeaders != null && customHeaders.Any())
            {
                customHeaders.Each((customHeader) => headers.TryAddWithoutValidation(customHeader.Key, customHeader.Value));
            }
        }
    }

    internal static class LightHttpServiceExtensions
    {
        /// <summary>
        /// Generates the http content for request..
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>the http content.</returns>
        public static HttpContent GenerateContent<TRequest>(this TRequest request, ContentTypeFormat contentType)
        {
            IContentTypeGenerator contentTypeGenerator;
            switch (contentType)
            {
                case ContentTypeFormat.FormData:
                    contentTypeGenerator = new FormContentTypeGenerator();
                    break;
                case ContentTypeFormat.MultipartFormData:
                    contentTypeGenerator = new MultipartContentTypeGenerator();
                    break;
                case ContentTypeFormat.Xml:
                    contentTypeGenerator = new XmlContentTypeGenerator();
                    break;
                default:
                    contentTypeGenerator = new JsonContentTypeGenerator();
                    break;
            }
            return contentTypeGenerator.GenerateRequestContent(request);
        }
    }
}
