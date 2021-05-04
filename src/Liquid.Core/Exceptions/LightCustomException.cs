using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.Exceptions
{
    /// <summary>
    /// Class responsible for custom exception codes handling.
    /// </summary>
    /// <seealso cref="LightException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class LightCustomException : LightException
    {
        /// <summary>
        /// Gets the response code.
        /// </summary>
        /// <value>
        /// The response code.
        /// </value>
        public ExceptionCustomCodes ResponseCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightCustomException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="responseCode">The response code.</param>
        public LightCustomException(string message, ExceptionCustomCodes responseCode) : base(message)
        {
            ResponseCode = responseCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightCustomException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="responseCode">The response code.</param>
        /// <param name="innerException">The inner exception.</param>
        public LightCustomException(string message, ExceptionCustomCodes responseCode, Exception innerException) : base(message, innerException)
        {
            ResponseCode = responseCode;
        }
    }
}