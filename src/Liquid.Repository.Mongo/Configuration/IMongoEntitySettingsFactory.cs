using Liguid.Repository.Configuration;
using Liquid.Repository.Mongo.Exceptions;
using System;

namespace Liquid.Repository.Mongo.Configuration
{
    /// <summary>
    /// Mongo Db Entity settings factory interface.
    /// </summary>
    public interface IMongoEntitySettingsFactory
    {
        /// <summary>
        /// Gets or creates an instance of <see cref="MongoEntitySettings"/> using the name of an Entity.
        /// </summary>
        /// <param name="entityName">The name of an specific Entity.</param>
        /// <exception cref="ArgumentNullException">
        /// entityName
        /// </exception>
        /// <exception cref="MongoEntitySettingsDoesNotExistException">
        /// When the configuration section name for the entity isn't found or malformed.
        /// </exception>
        /// <exception cref="LiquidDatabaseSettingsDoesNotExistException">
        /// When the configuration section name for the database referenced on the entity settings isn't found or malformed.
        /// </exception>
        MongoEntitySettings GetSettings(string entityName);

        /// <summary>
        /// Gets or creates  an instance of <see cref="MongoEntitySettings"/> using the type name of an Entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of an specific Entity.</typeparam>
        MongoEntitySettings GetSettings<TEntity>();
    }
}