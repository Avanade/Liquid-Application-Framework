using MongoDB.Driver;

namespace Liquid.Repository.MongoDb
{
    /// <summary>
    /// Mongo database context interface.
    /// </summary>
    /// <seealso cref="Liquid.Repository.ILightDataContext" />
    public interface IMongoDbDataContext : ILightDataContext
    {
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
        /// Sets an instance of <see cref="MongoDatabaseBase"/> into de property Database, 
        /// which is obtained from MongoClient by database name.<see cref="MongoClient.GetDatabase(string, MongoDatabaseSettings)"/>.
        /// </summary>
        /// <param name="databaseName">The name of database.</param>
        void SetDatabase(string databaseName);
    }
}
