using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Liquid.Services.Http.ContentTypes
{
    /// <summary>
    /// xml format content type generator.
    /// </summary>
    /// <seealso cref="Liquid.Services.Http.ContentTypes.IContentTypeGenerator" />
    internal class XmlContentTypeGenerator : IContentTypeGenerator
    {
        /// <summary>
        /// Generates the content of the request in xml format.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request object.</param>
        /// <returns>
        /// The Http content with data.
        /// </returns>
        public HttpContent GenerateRequestContent<TRequest>(TRequest request)
        {
            string contentString;
            var xmlserializer = new XmlSerializer(typeof(TRequest));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, request);
                contentString = stringWriter.ToString();
            }
            HttpContent requestContent = new StringContent(contentString, Encoding.UTF8);
            requestContent.Headers.Clear();
            requestContent.Headers.Add("content-type", "application/xml");
            return requestContent;
        }

        /// <summary>
        /// Generates the content of the response.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <returns>the object based on response</returns>
        public async Task<TResponse> GenerateResponseContent<TResponse>(HttpResponseMessage httpResponse)
        {
            try
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                var xmlserializer = new XmlSerializer(typeof(TResponse));
                TResponse result;
                using (TextReader reader = new StringReader(contentString))
                {
                    result = (TResponse)xmlserializer.Deserialize(reader);
                }
                return result;
            }
            catch
            {
                return default;
            }
        }
    }
}