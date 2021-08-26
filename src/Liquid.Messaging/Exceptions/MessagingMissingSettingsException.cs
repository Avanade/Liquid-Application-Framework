using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Messaging.Exceptions
{
    /// <summary>
    /// Occurs when a Configuration in settings configuration does not exist.
    /// </summary>
    /// <seealso cref="LiquidException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MessagingMissingSettingsException : LiquidException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MessagingMissingSettingsException"/>
        /// </summary>
        /// <param name="settingsName">Configuration set name.</param>
        public MessagingMissingSettingsException(string settingsName) : base($"The messaging configuration settings name {settingsName} does not exist. Please check your configuration file.")
        {
        }

        ///<inheritdoc/>
        public MessagingMissingSettingsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected MessagingMissingSettingsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
