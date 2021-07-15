using Liquid.Repository.Mongo.Attributes;
using MongoDB.Driver;

namespace Liquid.Repository.Mongo
{
    /// <summary>
    /// Mongo database context interface.
    /// </summary>
    /// <seealso cref="Liquid.Repository.ILiquidDataContext" />
    public interface IMongoDataContext<TEntity> : ILiquidDataContext
    {
        /// <summary>
        /// Gets configurations set from <typeparamref name="TEntity"/> attribute.
        /// </summary>
        MongoAttribute Settings { get; }
        /// <summary>
        /// Gets the Mongo Database.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        IMongoDatabase Database { get; }

        /// <summary>
        /// Gets the mongo client.
        /// </summary>
        /// <value>
        /// The mongo client.
        /// </value>
        IMongoClient MongoClient { get; }

        /// <summary>
        /// Gets the mongo session handle.
        /// </summary>
        IClientSessionHandle ClientSessionHandle { get; }

        /// <summary>
        /// Sets an instance of <see cref="MongoDatabaseBase"/> into de property Database, 
        /// which is obtained from MongoClient by database name.<see cref="MongoClient.GetDatabase(string, MongoDatabaseSettings)"/>.
        /// </summary>
        /// <param name="databaseName">The name of database.</param>
        void SetDatabase(string databaseName);
    }
}
