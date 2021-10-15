using Liquid.Repository.Configuration;

namespace Liquid.Repository.Mongo.Configuration
{
    /// <summary>
    /// Interface for MongoDB repository data entity settings.
    /// </summary>
    public interface IMongoEntitySettings
    {
        /// <summary>
        /// Gets or sets the name of the collection where an entity is persisted.
        /// </summary>
        /// <value>
        /// The name of the collection.
        /// </value>
        string CollectionName { get; set; }

        /// <summary>
        /// Gets or sets the partition (shard) key.
        /// </summary>
        /// <value>
        /// The partition (shard) key.
        /// </value>
        string ShardKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the configuration section where the Mongo DB settings are configured.
        /// </summary>
        /// <value>
        /// The name of the Database settings configuration section related to one or more entities.
        /// </value>
        string DatabaseSettingsSectionName { get; set; }

        /// <summary>
        /// Gets or sets the Mongo DB database settings.
        /// </summary>
        /// <value>
        /// The database settings.
        /// </value>
        DatabaseSettings DatabaseSettings { get; set; }
    }
}