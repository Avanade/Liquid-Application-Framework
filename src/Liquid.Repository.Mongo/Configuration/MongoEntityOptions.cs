using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.Mongo.Configuration
{
    /// <summary>
    /// Mongo Db data entity options for repository.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MongoEntityOptions
    {
        /// <summary>
        /// Gets or sets the name of the collection.
        /// </summary>
        /// <value>
        /// The name of the collection.
        /// </value>
        public string CollectionName { get; set; }

        /// <summary>
        /// Gets or sets the partition key.
        /// </summary>
        /// <value>
        /// The partition key.
        /// </value>
        public string ShardKey { get; set; }

        /// <summary>
        /// Gets or sets the name of database.
        /// </summary>
        /// <value>
        /// The name of database.
        /// </value>
        public string DatabaseName { get; set; }

    }
}
