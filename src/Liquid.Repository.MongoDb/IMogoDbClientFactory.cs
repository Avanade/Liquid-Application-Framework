using Liguid.Repository.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Liquid.Repository.MongoDb
{
    /// <summary>
    /// Provide client generator methods.
    /// </summary>
    public interface IMongoDbClientFactory
    {
        /// <summary>
        /// Provide a new instance of <see cref="MongoClient"/> with db conection started.
        /// </summary>
        /// <param name="connectionId">data base configuration Id.</param>
        /// <returns></returns>
        IMongoClient GetClient(string connectionId);
    }
}
