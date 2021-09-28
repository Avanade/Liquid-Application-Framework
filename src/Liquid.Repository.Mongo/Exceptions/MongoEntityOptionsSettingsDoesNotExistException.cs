using Liquid.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Liquid.Repository.Mongo.Exceptions
{
    /// <summary>
    /// Occurs when the Mongo Entity Options aren't not found in appsettings file. Check the configuration section name and the configuration file.
    /// </summary>
    /// <seealso cref="Liquid.Core.Exceptions.LiquidException" />
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class MongoEntityOptionsSettingsDoesNotExistException : LiquidException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoEntityOptionsSettingsDoesNotExistException"/> class.
        /// </summary>
        /// <param name="sectionName">The configuration section identifier.</param>
        public MongoEntityOptionsSettingsDoesNotExistException(string sectionName)
            : base($"The Mongo Entity Options for configuration section name '{sectionName}' does not exist.")
        {
        }
        ///<inheritdoc/>
        public MongoEntityOptionsSettingsDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc/>
        protected MongoEntityOptionsSettingsDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}