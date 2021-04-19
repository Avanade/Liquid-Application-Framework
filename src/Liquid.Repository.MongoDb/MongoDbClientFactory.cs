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
        public IMongoClient GetClient(LightConnectionSettings connectionSettings)
        {
            var mongoClient = new MongoClient(connectionSettings.ConnectionString);

            var database = mongoClient.GetDatabase(connectionSettings.DatabaseName);

            if (database is null)
            {
                throw new MongoDatabaseDoesNotExistException(connectionSettings.DatabaseName);
            }

            return mongoClient;
        }
    }
}
