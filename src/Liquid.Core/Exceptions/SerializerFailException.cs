using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Core.Exceptions
{
    /// <summary>
    /// Occurs when the serialization fail.
    /// </summary>
    /// <seealso cref="LiquidException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class SerializerFailException : LiquidException
    {
        ///<inheritdoc/>
        public SerializerFailException(string message) : base(message)
        {
        }

        ///<inheritdoc/>
        public SerializerFailException(string nameOfContent, Exception innerException) : base($"An error occurred whilst serializing of content {nameOfContent} : ", innerException)
        {
        }

        ///<inheritdoc/>
        protected SerializerFailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
