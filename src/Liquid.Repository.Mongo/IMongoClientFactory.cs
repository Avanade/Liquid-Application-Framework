using Liquid.Repository.Configuration;
using MongoDB.Driver;

namespace Liquid.Repository.Mongo
{
    /// <summary>
    /// Provide client generator methods.
    /// </summary>
    public interface IMongoClientFactory
    {
        /// <summary>
        /// Provide a new instance of <see cref="MongoClient"/> with db conection started.
        /// </summary>
        /// <param name="databaseSettings">Database settings used to create unique clients based on DatabaseSettings hash code.</param>
        IMongoClient GetClient(DatabaseSettings databaseSettings);
    }
}
