using System;

namespace Liquid.Core.Exceptions
{
    [Serializable]
    internal class DataMappingException : LiquidException
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

    }
}
