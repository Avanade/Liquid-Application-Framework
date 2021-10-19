using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Messaging.Exceptions
{
    ///<inheritdoc/>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class MessagingMissingScopedKeysException : LiquidException
    {
        ///<inheritdoc/>
        public MessagingMissingScopedKeysException()
        {
        }

        ///<inheritdoc/>
        public MessagingMissingScopedKeysException(string contextKey) : base($"The value of required logging scoped key '{contextKey}' was not found in request.")
        {
        }

        ///<inheritdoc/>
        public MessagingMissingScopedKeysException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected MessagingMissingScopedKeysException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
