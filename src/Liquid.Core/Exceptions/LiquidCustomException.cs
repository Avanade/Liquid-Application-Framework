using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Core.Exceptions
{
    /// <summary>
    /// Class responsible for custom exception codes handling.
    /// </summary>
    /// <seealso cref="LiquidException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class LiquidCustomException : LiquidException
    {
        /// <summary>
        /// Gets the response code.
        /// </summary>
        /// <value>
        /// The response code.
        /// </value>
        public ExceptionCustomCodes ResponseCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidCustomException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="responseCode">The response code.</param>
        public LiquidCustomException(string message, ExceptionCustomCodes responseCode) : base(message)
        {
            ResponseCode = responseCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidCustomException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="responseCode">The response code.</param>
        /// <param name="innerException">The inner exception.</param>
        public LiquidCustomException(string message, ExceptionCustomCodes responseCode, Exception innerException) : base(message, innerException)
        {
            ResponseCode = responseCode;
        }

        ///<inheritdoc/>
        protected LiquidCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}