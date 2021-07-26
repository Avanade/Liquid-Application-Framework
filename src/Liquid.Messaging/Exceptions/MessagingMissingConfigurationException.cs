using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Liquid.Core.Exceptions;

namespace Liquid.Messaging.Exceptions
{
    /// <summary>
    /// Occurs when a Configuration in settings configuration does not exist.
    /// </summary>
    /// <seealso cref="LiquidException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MessagingMissingConfigurationException : LiquidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingMissingConfigurationException"/> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        public MessagingMissingConfigurationException(string connectionId) : base($"The messaging configuration id {connectionId} does not exist. Please check your configuration file.")
        {
        }

        /// <inheritdoc/>
        protected MessagingMissingConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}