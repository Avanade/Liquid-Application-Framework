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
    }
}
