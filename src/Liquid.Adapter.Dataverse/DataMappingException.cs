using System.Runtime.Serialization;

namespace Liquid.Adapter.Dataverse
{
    [Serializable]
    internal class DataMappingException : Exception
    {
        public DataMappingException()
        {
        }

        public DataMappingException(string? message) : base(message)
        {
        }

        public DataMappingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DataMappingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}