using Liquid.Core.Exceptions;
using System;
using System.Runtime.Serialization;

namespace Liquid.WebApi.Http.Exceptions
{
    ///<inheritdoc/>
    public class LiquidContextKeysException : LiquidException
    {
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
