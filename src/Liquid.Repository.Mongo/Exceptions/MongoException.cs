using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.Mongo.Exceptions
{
    /// <summary>
    /// Occurs when an exception has occurred in Mongo Db.
    /// </summary>
    /// <seealso cref="LightException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MongoException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public MongoException(Exception innerException) : base("An error has occurred in database command. Please see inner exception", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MongoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
