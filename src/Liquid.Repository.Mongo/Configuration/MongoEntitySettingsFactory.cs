using Liguid.Repository.Configuration;
using Liquid.Repository.Configuration;
using Liquid.Repository.Mongo.Exceptions;
using Microsoft.Extensions.Configuration;
using System;

namespace Liquid.Repository.Mongo.Configuration
{
    /// <summary>
    /// Mongo Db Entity settings factory implementation.
    /// </summary>
    public class MongoEntitySettingsFactory : IMongoEntitySettingsFactory
    {
        private readonly IConfiguration _repositorySettings;
        private readonly string _entitiesConfigurationRootSectionName = "Liquid:RepositorySettings:Entities:{0}";

        /// <summary>
        /// Mongo Db Entity settings factory constructor.
        /// </summary>
        /// <param name="repositorySettings">The configuration used to create <see cref="MongoEntitySettings"/>.</param>
        /// <param name="entitiesConfigurationRootSectionName">Name of the configuration section where all entities have their repository settings configured. Default: "Liquid:RepositorySettings:Entities".</param>
        /// <exception cref="ArgumentNullException">
        /// repositorySettings
        /// </exception>
        public MongoEntitySettingsFactory(IConfiguration repositorySettings, string entitiesConfigurationRootSectionName = "Liquid:RepositorySettings:Entities")
        {
            if (repositorySettings is null) throw new ArgumentNullException(nameof(repositorySettings));
            if (!string.IsNullOrEmpty(entitiesConfigurationRootSectionName)) _entitiesConfigurationRootSectionName = entitiesConfigurationRootSectionName;

            if (!_entitiesConfigurationRootSectionName.EndsWith(":{0}")) _entitiesConfigurationRootSectionName = string.Concat(_entitiesConfigurationRootSectionName, ":{0}");

            _repositorySettings = repositorySettings;
        }

        ///<inheritdoc/>
        public MongoEntitySettings GetSettings<TEntity>()
        {
            return GetSettings(typeof(TEntity).Name);
        }

        ///<inheritdoc/>
        public MongoEntitySettings GetSettings(string entityName)
        {
            if (string.IsNullOrEmpty(entityName)) throw new ArgumentNullException(nameof(entityName));

            var entityConfigurationSectionName = string.Format(_entitiesConfigurationRootSectionName, entityName);

            // MongoEntitySettings will be retrieved from the configuration providers using the configuration section name
            var entitySettings = _repositorySettings.GetSection(entityConfigurationSectionName)?.Get<MongoEntitySettings>();
            if (entitySettings is null) throw new MongoEntitySettingsDoesNotExistException(entityName);

            // DatabaseSettings will be retrieved from the configuration providers using the configuration section name configured for an Entity
            // or the default configuration section name. See GetDefaultDatabaseConfigurationSectionName() private method below.
            var databaseConfigurationSectionName = entitySettings.DatabaseSettingsSectionName ?? GetDefaultDatabaseConfigurationSectionName();
            entitySettings.DatabaseSettings = _repositorySettings.GetSection(databaseConfigurationSectionName)?.Get<DatabaseSettings>();
            if (entitySettings.DatabaseSettings is null) throw new LiquidDatabaseSettingsDoesNotExistException(databaseConfigurationSectionName);

            return entitySettings;
        }

        /// <summary>
        /// Returns a section name, based on _entitiesConfigurationRootSectionName, trying to going up two levels and concatenating ":DefaultDatabaseSettings".
        /// Samples:
        ///         if _entitiesConfigurationRootSectionName = "Liquid:RepositorySettings:Entities:{0}" it will return "Liquid:RepositorySettings:DefaultDatabaseSettings"
        ///         if _entitiesConfigurationRootSectionName = "MyRepository:Entities:{0}" it will return "MyRepository:DefaultDatabaseSettings"
        ///         if _entitiesConfigurationRootSectionName = "MyEntities:{0}" it will return "DefaultDatabaseSettings"
        /// </summary>
        private string GetDefaultDatabaseConfigurationSectionName()
        {
            var sectionElements = _entitiesConfigurationRootSectionName.Split(':');

            if (sectionElements.Length < 3)
                return "DefaultDatabaseSettings";

            Array.Resize<string>(ref sectionElements, sectionElements.Length - 2);
            return string.Concat(string.Join(':', sectionElements), ":DefaultDatabaseSettings");
        }
    }
}
