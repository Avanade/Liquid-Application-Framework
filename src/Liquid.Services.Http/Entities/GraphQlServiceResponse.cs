using System;
using System.Net.Http;
using System.Threading.Tasks;
using Liquid.Services.Http.Exceptions;
using Newtonsoft.Json.Linq;

namespace Liquid.Services.Http.Entities
{
    /// <summary>
    /// The response object from a Graph QL request.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public class GraphQlServiceResponse<TResponse>
    {
        /// <summary>
        /// Gets the HTTP response.
        /// </summary>
        /// <value>
        /// The HTTP response.
        /// </value>
        public HttpResponseMessage HttpResponse { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphQlServiceResponse{TResponse}"/> class.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        public GraphQlServiceResponse(HttpResponseMessage httpResponse)
        {
            HttpResponse = httpResponse;
        }

        /// <summary>
        /// Gets the content object from "data" property.
        /// </summary>
        /// <value>
        /// The content object.
        /// </value>
        public async Task<TResponse> GetContentObjectAsync()
        {
            try
            {
                var contentString = await HttpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jObject = JObject.Parse(contentString);
                var responseObject = jObject["data"].ToObject<TResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                if (HttpResponse != null)
                {
                    throw new HttpServiceCallException(ex, HttpResponse.RequestMessage?.RequestUri?.ToString(), HttpResponse.RequestMessage?.Method?.Method);
                }
                throw new HttpServiceCallException(ex);
            }
        }

        /// <summary>
        /// Gets the content object.
        /// </summary>
        /// <param name="node">A specific node from return.</param>
        /// <returns></returns>
        /// <value>
        /// The content object.
        /// </value>
        public async Task<TResponse> GetContentObjectAsync(string node)
        {
            try
            {
                var contentString = await HttpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jObject = JObject.Parse(contentString);
                var responseObject = jObject["data"][node].ToObject<TResponse>();
                return responseObject;
            }
            catch (Exception ex)
            {
                if (HttpResponse != null)
                {
                    throw new HttpServiceCallException(ex, HttpResponse.RequestMessage?.RequestUri?.ToString(), HttpResponse.RequestMessage?.Method?.Method);
                }
                throw new HttpServiceCallException(ex);
            }
        }
    }
}