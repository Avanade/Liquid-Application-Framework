using Liquid.Core.Settings;
using Liquid.Repository.Mongo.Configuration;
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
        /// <param name="collectionName"></param>
        /// <param name="settings"></param>
        IMongoClient GetClient(string collectionName, out MongoEntitySettings settings);
    }
}
