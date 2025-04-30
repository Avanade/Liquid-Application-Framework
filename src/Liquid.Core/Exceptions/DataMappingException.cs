using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Core.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    internal class DataMappingException : Exception
    {
        public DataMappingException()
        {
        }

        public DataMappingException(string message) : base(message)
        {
        }

        public DataMappingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DataMappingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}