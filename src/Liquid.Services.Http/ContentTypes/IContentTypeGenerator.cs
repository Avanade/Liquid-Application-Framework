using System.Net.Http;
using System.Threading.Tasks;

namespace Liquid.Services.Http.ContentTypes
{
    /// <summary>
    /// Http content type generator interface.
    /// </summary>
    public interface IContentTypeGenerator
    {
        /// <summary>
        /// Generates the content of the request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>The Http content with data.</returns>
        HttpContent GenerateRequestContent<TRequest>(TRequest request);

        /// <summary>
        /// Generates the content of the response.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <returns></returns>
        Task<TResponse> GenerateResponseContent<TResponse>(HttpResponseMessage httpResponse);
    }
}