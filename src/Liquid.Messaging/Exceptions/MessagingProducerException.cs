using System;
using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Exceptions;

namespace Liquid.Messaging.Exceptions
{
    /// <summary>
    /// Occurs when an exception occurs sending a message.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LightException" />
    [ExcludeFromCodeCoverage]
    public class MessagingProducerException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingProducerException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public MessagingProducerException(Exception innerException) : base("An error has occurred when sending message. See inner exception for more detail.", innerException)
        {
        }
    }
}