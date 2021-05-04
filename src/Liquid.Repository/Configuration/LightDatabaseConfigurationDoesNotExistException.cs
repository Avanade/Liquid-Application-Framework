using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liguid.Repository.Configuration
{
    /// <summary>
    /// Occurs when the database connection string is not found in appsettings file. Check the connection id and the configuration file.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LightException" />
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class LightDatabaseConfigurationDoesNotExistException : LightException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightDatabaseConfigurationDoesNotExistException"/> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        public LightDatabaseConfigurationDoesNotExistException(string connectionId)
            : base($"Database connection string '{connectionId}' does not exist.")
        {
        }
        ///<inheritdoc/>
        public LightDatabaseConfigurationDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected LightDatabaseConfigurationDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
