using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Grpc.Core;

namespace Liquid.WebApi.Grpc.Extensions
{
    /// <summary>
    /// Http context extensions class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServerCallContextExtensions
    {
        /// <summary>
        /// Gets the header value from request.
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <param name="headerKey">The header key.</param>
        /// <returns>Header value from http request, otherwise <see cref="string.Empty" />.</returns>
        public static string GetHeaderValueFromRequest(this ServerCallContext context, string headerKey)
        {
            Metadata headerDictionary = null;
            if (context != null)
            {
                headerDictionary = context.RequestHeaders;
            }
            if (headerDictionary == null)
            {
                return string.Empty;
            }
            var stringValues = headerDictionary.FirstOrDefault(m => string.Equals(m.Key, headerKey, StringComparison.InvariantCultureIgnoreCase)).Value;
            if (string.IsNullOrEmpty(stringValues))
            {
                return string.Empty;
            }
            return stringValues;
        }
    }
}