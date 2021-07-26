using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Liquid.Core.Exceptions;

namespace Liquid.Messaging.Exceptions
{
    /// <summary>
    /// Occurs when an exception occurs sending a message.
    /// </summary>
    /// <seealso cref="LiquidException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MessagingProducerException : LiquidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingProducerException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public MessagingProducerException(Exception innerException) : base("An error has occurred when sending message. See inner exception for more detail.", innerException)
        {
        }

        /// <inheritdoc/>
        protected MessagingProducerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}