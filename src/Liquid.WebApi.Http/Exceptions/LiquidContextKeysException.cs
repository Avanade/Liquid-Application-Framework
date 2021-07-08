using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.WebApi.Http.Exceptions
{
    ///<inheritdoc/>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class LiquidContextKeysException : LiquidException
    {
        ///<inheritdoc/>
        public LiquidContextKeysException()
        {
        }

        ///<inheritdoc/>
        public LiquidContextKeysException(string contextKey) : base($"The value of required context key '{contextKey}' was not found in request.")
        {
        }

        ///<inheritdoc/>
        public LiquidContextKeysException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected LiquidContextKeysException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
