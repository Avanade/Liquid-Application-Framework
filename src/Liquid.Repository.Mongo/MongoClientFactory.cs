using Liquid.Repository.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Liquid.Repository.Mongo
{
    ///<inheritdoc/>
    public class MongoClientFactory : IMongoClientFactory
    {
        private readonly IDictionary<int, IMongoClient> _mongoClients;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoClientFactory" /> class.
        /// </summary>
        public MongoClientFactory()
        {
            _mongoClients = new Dictionary<int, IMongoClient>();
        }

        ///<inheritdoc/>
        public IMongoClient GetClient(DatabaseSettings databaseSettings)
        {
            if (databaseSettings is null) throw new ArgumentNullException(nameof(databaseSettings));

            // Try to get from the created clients collection, otherwise creates a new client
            IMongoClient mongoClient = _mongoClients.TryGetValue(databaseSettings.GetHashCode(), out mongoClient) ? mongoClient : CreateClient(databaseSettings);

            return mongoClient;
        }

        private IMongoClient CreateClient(DatabaseSettings databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.ConnectionString);

            _mongoClients.Add(databaseSettings.GetHashCode(), mongoClient);

            return mongoClient;
        }
    }
}
