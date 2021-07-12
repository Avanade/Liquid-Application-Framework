using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Repository.EntityFramework.Exceptions
{
    /// <summary>
    /// Occurs when the database is not found in Sql Server.
    /// </summary>
    /// <seealso cref="LiquidException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class DatabaseDoesNotExistException : LiquidException
    {
        ///<inheritdoc/>
        public DatabaseDoesNotExistException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDoesNotExistException"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        public DatabaseDoesNotExistException(string databaseName) : base($"Database {databaseName} does not exist. Please check name or create a new database")
        {
        }

        ///<inheritdoc/>
        public DatabaseDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected DatabaseDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}