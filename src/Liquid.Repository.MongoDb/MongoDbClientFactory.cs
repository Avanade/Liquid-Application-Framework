using Liguid.Repository.Configuration;
using Liquid.Repository.MongoDb.Exceptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Liquid.Repository.MongoDb
{
    ///<inheritdoc/>
    public class MongoDbClientFactory : IMongoDbClientFactory
    {
        ///<inheritdoc/>
        public IMongoClient GetClient(IOptions<LightConnectionSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);

            var database = mongoClient.GetDatabase(options.Value.DatabaseName);

            if (database is null)
            {
                throw new MongoDatabaseDoesNotExistException(options.Value.DatabaseName);
            }

            return mongoClient;
        }
    }
}
