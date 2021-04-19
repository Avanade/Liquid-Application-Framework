using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.MongoDb.Attributes
{
    /// <summary>
    /// Mongo Db data Attributes for repository.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    [ExcludeFromCodeCoverage]
    public class MongoDbAttribute : Attribute
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbAttribute" /> class.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="shardKey">The shard key.</param>
        /// <param name="databaseName">Name of the collection.</param>
        /// <exception cref="System.ArgumentNullException">collectionName
        /// or
        /// partitionKey</exception>
        public MongoDbAttribute(string collectionName, string shardKey, string databaseName)
        {
            CollectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            ShardKey = shardKey ?? throw new ArgumentNullException(nameof(shardKey));
            DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        }
    }
}
