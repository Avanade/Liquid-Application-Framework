using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Liquid.Core.Exceptions;

namespace Liquid.Cache
{
    /// <summary>
    /// Occurs when an error has occurred during cache manipulation.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class LightCacheException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightCacheException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LightCacheException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightCacheException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="ex">The ex.</param>
        public LightCacheException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightCacheException"/> class.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public LightCacheException(Exception innerException) : base("An error has occurred accessing cache. See inner exception for more detail.", innerException)
        {
        }

        /// <inheritdoc/> 
        protected LightCacheException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}