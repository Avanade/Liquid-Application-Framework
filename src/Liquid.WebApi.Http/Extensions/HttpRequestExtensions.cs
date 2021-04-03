using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Liquid.WebApi.Http.Extensions
{
    /// <summary>
    /// Http Request Extensions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Gets the value from querystring.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetValueFromQuerystring(this HttpRequest request, string key)
        {
            var queryCollection = request?.Query;
            if (queryCollection == null) return string.Empty;
            var stringValues = queryCollection.FirstOrDefault(m => string.Equals(m.Key, key, StringComparison.InvariantCultureIgnoreCase)).Value;
            if (string.IsNullOrEmpty(stringValues)) return string.Empty;
            return stringValues;
        }

        /// <summary>
        /// Gets the request body.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        internal static async Task<string> GetRequestBody(this HttpRequest request)
        {
            var injectedRequestStream = new MemoryStream();
            
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //Re write the stream back to http body.
            var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
            injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
            injectedRequestStream.Seek(0, SeekOrigin.Begin);
            request.Body = injectedRequestStream;

            return bodyAsText;
        }

        /// <summary>
        /// Gets the response body.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="responseBuffer">The response buffer.</param>
        /// <returns></returns>
        internal static async Task<string> GetResponseBody(this HttpResponse response, Stream stream, MemoryStream responseBuffer)
        {
            responseBuffer.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBuffer).ReadToEnd();
            responseBuffer.Seek(0, SeekOrigin.Begin);
            await responseBuffer.CopyToAsync(stream);
            return responseBody;
        }
    }
}