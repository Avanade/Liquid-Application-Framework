using Liquid.Services.Http.ContentTypes;
using Liquid.Services.Http.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Liquid.Services.Http.Entities
{
    /// <summary>
    /// The response message from api call.
    /// </summary>
    /// <typeparam name="TResponse">The object type of the response.</typeparam>
    public class HttpServiceResponse<TResponse>
    {
        /// <summary>
        /// Gets the HTTP response.
        /// </summary>
        /// <value>
        /// The HTTP response.
        /// </value>
        public HttpResponseMessage HttpResponse { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServiceResponse{TResponse}"/> class.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        public HttpServiceResponse(HttpResponseMessage httpResponse)
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
                IContentTypeGenerator contentTypeGenerator;
                if (HttpResponse.Content.Headers.Contains("content-type"))
                {
                    var contentType = HttpResponse.Content.Headers.GetValues("content-type").First();
                    if (contentType.Contains("application/json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        contentTypeGenerator = new JsonContentTypeGenerator();
                    }
                    else if (contentType.Contains("application/xml", StringComparison.InvariantCultureIgnoreCase) ||
                             contentType.Contains("text/xml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        contentTypeGenerator = new XmlContentTypeGenerator();
                    }
                    else
                    {
                        contentTypeGenerator = new JsonContentTypeGenerator();
                    }
                    var responseObject = await contentTypeGenerator.GenerateResponseContent<TResponse>(HttpResponse);
                    return responseObject;
                }
                else
                {
                    contentTypeGenerator = new JsonContentTypeGenerator();
                    var responseObject = await contentTypeGenerator.GenerateResponseContent<TResponse>(HttpResponse);
                    return responseObject;
                }
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
        /// Gets the exception content in string format when the http response status is 500 (Internal server error).
        /// </summary>
        /// <value>
        /// The exception content.
        /// </value>
        [ExcludeFromCodeCoverage]
        public string ExceptionContent => HttpResponse.StatusCode == HttpStatusCode.InternalServerError ? HttpResponse.Content.ReadAsStringAsync().Result : null;
    }
}