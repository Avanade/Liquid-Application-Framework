using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Liquid.WebApi.Http.Extensions
{
    /// <summary>
    /// Http context extensions class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets the header value from request.
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <param name="headerKey">The header key.</param>
        /// <returns>Header value from http request, otherwise <see cref="string.Empty" />.</returns>
        public static string GetHeaderValueFromRequest(this HttpContext context, string headerKey)
        {
            IHeaderDictionary headerDictionary = null;
            if (context != null)
            {
                headerDictionary = context.Request?.Headers;
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