using Liquid.Services.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Liquid.Services.Http.ContentTypes
{
    /// <summary>
    /// Form Content Type Generator Class.
    /// </summary>
    /// <seealso cref="Liquid.Services.Http.ContentTypes.IContentTypeGenerator" />
    internal class FormContentTypeGenerator : IContentTypeGenerator
    {
        /// <summary>
        /// Generates the content of the request form content.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>The Http content with data.</returns>
        public HttpContent GenerateRequestContent<TRequest>(TRequest request)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            var properties = request.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetGetMethod() != null && propertyInfo.CanRead)
                {
                    var fieldAttribute = propertyInfo.GetCustomAttribute<FormFieldAttribute>();
                    var fieldName = fieldAttribute != null ? fieldAttribute.FieldName : propertyInfo.Name;
                    var fieldValue = propertyInfo.GetValue(request) ?? string.Empty;
                    parameters.Add(fieldName, fieldValue.ToString());
                }
            }
            HttpContent requestContent = new FormUrlEncodedContent(parameters);
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