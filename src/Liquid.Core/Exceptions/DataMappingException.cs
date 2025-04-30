using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Core.Exceptions
{
    /// <summary>
    /// Occurs when an exception is raised during data mapping.
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class DataMappingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataMappingException"/> class.
        /// </summary>
        public DataMappingException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMappingException"/> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public DataMappingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMappingException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DataMappingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataMappingException"/> class with serialized data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public DataMappingException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}