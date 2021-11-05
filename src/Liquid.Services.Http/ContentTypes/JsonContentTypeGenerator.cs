using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Liquid.Services.Http.ContentTypes
{
    /// <summary>
    /// Json content type Generator class. Generates http requests with content type json.
    /// </summary>
    /// <seealso cref="Liquid.Services.Http.ContentTypes.IContentTypeGenerator" />
    internal class JsonContentTypeGenerator : IContentTypeGenerator
    {
        /// <summary>
        /// Generates the content of the request in json format.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>Json http content</returns>
        public HttpContent GenerateRequestContent<TRequest>(TRequest request)
        {
            var contentString = JsonConvert.SerializeObject(request, Formatting.None) ?? string.Empty;
            HttpContent requestContent = new StringContent(contentString);
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return requestContent;
        }

        /// <summary>
        /// Generates the content of the response.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <returns>the object based on response</returns>
        public async Task<TResponse> GenerateResponseContent<TResponse>(HttpResponseMessage httpResponse)
        {
            var contentString = await httpResponse.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<TResponse>(contentString);
            return responseObject;
        }
    }
}