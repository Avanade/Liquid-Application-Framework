using System;
using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Exceptions;

namespace Liquid.Services.Http.Exceptions
{
    /// <summary>
    /// Occurs if the service has return an error.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [ExcludeFromCodeCoverage]
    public class HttpServiceCallException : LightException
    {
        private const string ExceptionMessage = "An error has occurred while accessing http request {0}.";

        /// <summary>
        /// The default exception message
        /// </summary>
        public const string DefaultExceptionMessage = "An exception has occurred during a http connection.";

        /// <summary>
        /// Gets the URL address of service where the error has occurred.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; }

        /// <summary>
        /// Gets the HTTP verb.
        /// </summary>
        /// <value>
        /// The HTTP verb.
        /// </value>
        public string HttpVerb { get; }

        /// <summary>
        /// Gets the HTTP request body.
        /// </summary>
        /// <value>
        /// The HTTP request body.
        /// </value>
        public string HttpRequestBody { get; }

        /// <summary>
        /// Gets the content of the service exception.
        /// </summary>
        /// <value>
        /// The content of the service exception.
        /// </value>
        public string ServiceExceptionContent { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServiceCallException" /> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public HttpServiceCallException(Exception exception) : base(DefaultExceptionMessage, exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServiceCallException"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="httpVerb">The HTTP verb.</param>
        /// <param name="httpRequestBody">The HTTP request body.</param>
        /// <param name="serviceExceptionContent">Content of the service exception.</param>
        public HttpServiceCallException(string url, string httpVerb, string httpRequestBody = null, string serviceExceptionContent = null) : base(string.Format(ExceptionMessage, url))
        {
            Url = url;
            HttpVerb = httpVerb;
            HttpRequestBody = httpRequestBody;
            ServiceExceptionContent = serviceExceptionContent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServiceCallException"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="url">The URL.</param>
        /// <param name="httpVerb">The HTTP verb.</param>
        /// <param name="httpRequestBody">The HTTP request body.</param>
        /// <param name="serviceExceptionContent">Content of the service exception.</param>
        public HttpServiceCallException(Exception exception, string url, string httpVerb, string httpRequestBody = null, string serviceExceptionContent = null) : base(string.Format(ExceptionMessage, url), exception)
        {
            Url = url;
            HttpVerb = httpVerb;
            HttpRequestBody = httpRequestBody;
            ServiceExceptionContent = serviceExceptionContent;
        }
    }
}