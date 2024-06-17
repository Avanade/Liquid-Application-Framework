using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Core.Exceptions
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

    }
}
