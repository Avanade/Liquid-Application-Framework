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
        /// <param name="databaseName">data base name.</param>
        /// <returns></returns>
        IMongoClient GetClient(string databaseName);
    }
}
