using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Messaging.Exceptions
{
    /// <summary>
    /// Occurs when an exception is raised consuming a message.
    /// </summary>
    /// <seealso cref="LiquidException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MessagingConsumerException : LiquidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingConsumerException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public MessagingConsumerException(Exception innerException) : base("An error has occurred consuming message. See inner exception for more detail.", innerException)
        {
        }

        /// <inheritdoc/>
        protected MessagingConsumerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}