using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.WebApi.Http.Exceptions
{
    ///<inheritdoc/>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class LiquidScopedtKeysException : LiquidException
    {
        ///<inheritdoc/>
        public LiquidScopedtKeysException()
        {
        }

        ///<inheritdoc/>
        public LiquidScopedtKeysException(string contextKey) : base($"The value of required logging scoped key '{contextKey}' was not found in request.")
        {
        }

        ///<inheritdoc/>
        public LiquidScopedtKeysException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected LiquidScopedtKeysException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
