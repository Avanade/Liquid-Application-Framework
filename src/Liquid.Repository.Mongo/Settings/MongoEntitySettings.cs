using Liquid.Core.Settings;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.Mongo.Settings
{
    /// <summary>
    /// Properties set list of service bus configurations.
    /// </summary>
    public class MongoDbSettings
    {
        /// <summary>
        /// Properties set list of service bus configurations.
        /// </summary>
        public List<MongoEntitySettings> Settings { get; set; }
    }

    /// <summary>
    /// MongoDB repository data entity settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MongoEntitySettings
    {
        ///<inheritdoc/>
        public string CollectionName { get; set; }

        ///<inheritdoc/>
        public string ShardKey { get; set; }

        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        /// <value>
        /// The database connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        public string DatabaseName { get; set; }
    }
}
