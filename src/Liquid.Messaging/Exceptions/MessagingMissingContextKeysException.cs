using Liquid.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;

namespace Liquid.Messaging.Exceptions
{
    ///<inheritdoc/>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class MessagingMissingContextKeysException : LiquidException
    {
        ///<inheritdoc/>
        public MessagingMissingContextKeysException()
        {
        }

        ///<inheritdoc/>
        public MessagingMissingContextKeysException(string contextKey) : base($"The value of required context key '{contextKey}' was not found in request.")
        {
        }

        ///<inheritdoc/>
        public MessagingMissingContextKeysException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected MessagingMissingContextKeysException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
