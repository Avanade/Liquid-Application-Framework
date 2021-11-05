using Liquid.Services.Http.Entities;
using Liquid.Services.Http.Enum;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Liquid.Services.Http
{
    /// <summary>
    /// Http service client interface.
    /// </summary>
    public interface ILightHttpService : ILightService
    {
        /// <summary>
        /// Executes the Get request against an endpoint url.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <returns>The Http response message with the necessary response.</returns>
        Task<HttpServiceResponse<TResponse>> GetAsync<TResponse>(string endpoint, Dictionary<string, string> customHeaders = null);

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
        Task<HttpServiceResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest request = default, Dictionary<string, string> customHeaders = null, ContentTypeFormat contentType = ContentTypeFormat.Json);

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
        Task<HttpServiceResponse<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest request = default, Dictionary<string, string> customHeaders = null, ContentTypeFormat contentType = ContentTypeFormat.Json);

        /// <summary>
        /// Deletes the specified resource in the url endpoint.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        Task<HttpServiceResponse<TResponse>> DeleteAsync<TResponse>(string endpoint, Dictionary<string, string> customHeaders = null);

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
        Task<HttpServiceResponse<TResponse>> DeleteAsync<TRequest, TResponse>(string endpoint, TRequest request = default, Dictionary<string, string> customHeaders = null, ContentTypeFormat contentType = ContentTypeFormat.Json);

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
        Task<HttpServiceResponse<TResponse>> SendRequestAsync<TRequest, TResponse>(string endpoint, HttpMethod method, TRequest request = default, Dictionary<string, string> customHeaders = null, ContentTypeFormat contentType = ContentTypeFormat.Json);

        /// <summary>
        /// Sends a custom stream request to an url endpoint. The request method could be any type (GET, POST, PUT, DELETE, OPTIONS, etc...)
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="method">The Http method (GET, POST, PUT, DELETE, OPTIONS, etc...).</param>
        /// <param name="stream">The content stream to be sent.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <returns>The Http response message with the necessary response.</returns>
        Task<HttpServiceResponse<TResponse>> SendStreamRequestAsync<TResponse>(string endpoint, HttpMethod method, Stream stream = null, Dictionary<string, string> customHeaders = null);

        /// <summary>
		/// Executes a GraphQL GET request against an endpoint url.
		/// </summary>
		/// <typeparam name="TResponse">The type of the response.</typeparam>
		/// <param name="endpoint">The url endpoint.</param>
		/// <param name="query">The GraphQL query.</param>
		/// <param name="customHeaders">The headers to be sent in request.</param>
		/// <returns>
		/// The Http response message with the necessary response.
		/// </returns>
		Task<GraphQlServiceResponse<TResponse>> GraphQlGetAsync<TResponse>(string endpoint, string query, Dictionary<string, string> customHeaders = null);

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
        Task<GraphQlServiceResponse<TResponse>> GraphQlGetAsync<TResponse>(string endpoint, GraphQlRequest request, Dictionary<string, string> customHeaders = null);

        /// <summary>
        /// Executes a GraphQL POST request against an endpoint url.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="endpoint">The url endpoint.</param>
        /// <param name="query">The GraphQL query.</param>
        /// <param name="customHeaders">The headers to be sent in request.</param>
        /// <returns>
        /// The Http response message with the necessary response.
        /// </returns>
        Task<GraphQlServiceResponse<TResponse>> GraphQlPostAsync<TResponse>(string endpoint, string query, Dictionary<string, string> customHeaders = null);

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
        Task<GraphQlServiceResponse<TResponse>> GraphQlPostAsync<TResponse>(string endpoint, GraphQlRequest request, Dictionary<string, string> customHeaders = null);
    }
}