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
        /// <param name="databaseId">Id of the database. Used to identify database settings and to create unique clients.</param>
        /// <returns></returns>
        IMongoClient GetClient(string databaseId);
    }
}
