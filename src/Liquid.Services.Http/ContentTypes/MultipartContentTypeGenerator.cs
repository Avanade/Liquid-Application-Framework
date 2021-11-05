using Liquid.Services.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Liquid.Services.Http.ContentTypes
{
    /// <summary>
    /// Multipart form data content type generator class.
    /// </summary>
    /// <seealso cref="Liquid.Services.Http.ContentTypes.IContentTypeGenerator" />
    [ExcludeFromCodeCoverage]
    internal class MultipartContentTypeGenerator : IContentTypeGenerator
    {
        /// <summary>
        /// Generates the content of the request.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The Http content with data.
        /// </returns>
        public HttpContent GenerateRequestContent<TRequest>(TRequest request)
        {
            var requestContent = new MultipartFormDataContent();

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            var properties = request.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetGetMethod() != null && propertyInfo.CanRead)
                {
                    if (propertyInfo.PropertyType == typeof(Stream))
                    {
                        if (propertyInfo.GetValue(request) is Stream streamValue)
                        {
                            requestContent.Add(new StreamContent(streamValue));
                        }
                    }
                    else
                    {
                        var fieldAttribute = propertyInfo.GetCustomAttribute<FormFieldAttribute>();
                        var fieldName = (fieldAttribute != null) ? fieldAttribute.FieldName : propertyInfo.Name;
                        var fieldValue = propertyInfo.GetValue(request) ?? string.Empty;
                        parameters.Add(fieldName, fieldValue.ToString());
                    }
                }
            }
            if (parameters.Any())
            {
                requestContent.Add(new FormUrlEncodedContent(parameters));
            }
            return requestContent;
        }

        /// <summary>
        /// Generates the content of the response.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <returns>
        /// the object based on response
        /// </returns>
        /// <exception cref="NotSupportedException">There is no response content format for content type 'multipart formData'.</exception>
        public Task<TResponse> GenerateResponseContent<TResponse>(HttpResponseMessage httpResponse)
        {
            throw new NotSupportedException("There is no response content format for content type 'multipart formData'.");
        }
    }
}