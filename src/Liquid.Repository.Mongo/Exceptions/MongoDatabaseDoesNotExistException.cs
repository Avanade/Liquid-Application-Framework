using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Repository.Mongo.Exceptions
{
    /// <summary>
    /// Occurs when the database is not found in Mongo Db server.
    /// </summary>
    /// <seealso cref="LightException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class MongoDatabaseDoesNotExistException : LightException
    {
        ///<inheritdoc/>
        public MongoDatabaseDoesNotExistException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDatabaseDoesNotExistException"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        public MongoDatabaseDoesNotExistException(string databaseName) : base($"Mongo database {databaseName} does not exist. Please check name or create a new database")
        {
        }

        ///<inheritdoc/>
        public MongoDatabaseDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected MongoDatabaseDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}