using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Repository.Mongo.Exceptions
{
    /// <summary>
    /// Occurs when the Mongo Entity Settings aren't not found in any configuration provider. Check the entity name and the configuration files.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LiquidException" />
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class MongoEntitySettingsDoesNotExistException : LiquidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoEntitySettingsDoesNotExistException"/> class.
        /// </summary>
        /// <param name="entityName">The entity name.</param>
        public MongoEntitySettingsDoesNotExistException(string entityName)
            : base($"The Mongo Entity Settings for entity '{entityName}' does not exist.")
        {
        }
        ///<inheritdoc/>
        public MongoEntitySettingsDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected MongoEntitySettingsDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}