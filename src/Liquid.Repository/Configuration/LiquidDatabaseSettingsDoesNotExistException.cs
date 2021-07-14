using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liguid.Repository.Configuration
{
    /// <summary>
    /// Occurs when the database connection string is not found in appsettings file. Check the connection id and the configuration file.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LiquidException" />
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class LiquidDatabaseSettingsDoesNotExistException : LiquidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidDatabaseSettingsDoesNotExistException"/> class.
        /// </summary>
        /// <param name="databaseName">The connection identifier.</param>
        public LiquidDatabaseSettingsDoesNotExistException(string databaseName)
            : base($"The connection string for database '{databaseName}' does not exist.")
        {
        }
        ///<inheritdoc/>
        public LiquidDatabaseSettingsDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected LiquidDatabaseSettingsDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
