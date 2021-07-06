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
        /// <param name="key">The header key.</param>
        /// <returns>Header value from http request, otherwise <see cref="string.Empty" />.</returns>
        public static string GetValueFromHeader(this HttpContext context, string key)
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
            var stringValues = headerDictionary.FirstOrDefault(m => string.Equals(m.Key, key, StringComparison.InvariantCultureIgnoreCase)).Value;
            if (string.IsNullOrEmpty(stringValues))
            {
                return string.Empty;
            }
            return stringValues;
        }

        /// <summary>
        /// Gets the value from querystring.
        /// </summary>
        /// <param name="context">The request.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetValueFromQuery(this HttpContext context, string key)
        {
            var queryCollection = context.Request?.Query;

            if (queryCollection == null)
                return string.Empty;

            var stringValues = queryCollection.FirstOrDefault(m => string.Equals(m.Key, key, StringComparison.InvariantCultureIgnoreCase)).Value;

            if (string.IsNullOrEmpty(stringValues)) 
                return string.Empty;

            return stringValues;
        }
    }
}