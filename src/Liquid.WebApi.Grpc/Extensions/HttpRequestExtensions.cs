using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Liquid.WebApi.Grpc.Extensions
{
    /// <summary>
    /// Http Request Extensions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HttpRequestExtensions
    {
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
    }
}