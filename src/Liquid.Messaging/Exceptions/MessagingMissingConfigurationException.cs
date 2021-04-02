using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Exceptions;

namespace Liquid.Messaging.Exceptions
{
    /// <summary>
    /// Occurs when a Configuration in settings configuration does not exist.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LightException" />
    [ExcludeFromCodeCoverage]
    public class MessagingMissingConfigurationException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingMissingConfigurationException"/> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        public MessagingMissingConfigurationException(string connectionId) : base($"The messaging configuration id {connectionId} does not exist. Please check your configuration file.")
        {
        }
    }
}