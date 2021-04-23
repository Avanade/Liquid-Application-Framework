using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.EntityFramework
{
    /// <summary>
    /// Occurs when the database is not found in Sql Server.
    /// </summary>
    /// <seealso cref="LightException" />
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class DatabaseDoesNotExistException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDoesNotExistException"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        public DatabaseDoesNotExistException(string databaseName) : base($"Database {databaseName} does not exist. Please check name or create a new database")
        {
        }
    }
}