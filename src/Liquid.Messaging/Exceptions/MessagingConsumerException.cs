using System;
using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Exceptions;

namespace Liquid.Messaging.Exceptions
{
    /// <summary>
    /// Occurs when an exception is raised consuming a message.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LightException" />
    [ExcludeFromCodeCoverage]
    public class MessagingConsumerException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingConsumerException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public MessagingConsumerException(Exception innerException) : base("An error has occurred consuming message. See inner exception for more detail.", innerException)
        {
        }
    }
}