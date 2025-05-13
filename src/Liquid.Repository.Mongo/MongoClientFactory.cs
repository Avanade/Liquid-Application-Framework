using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Exceptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.Repository.Mongo
{
    ///<inheritdoc/>
    public class MongoClientFactory : IMongoClientFactory
    {
        private readonly IOptions<MongoDbSettings> _settings;
        private readonly IDictionary<string, IMongoClient> _mongoClients;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoClientFactory" /> class.
        /// </summary>
        public MongoClientFactory(IOptions<MongoDbSettings> settings)
        {
            _mongoClients = new Dictionary<string, IMongoClient>();
            _settings = settings;
        }

        ///<inheritdoc/>
        public IMongoClient GetClient(string collectionName, out MongoEntitySettings settings)
        {
            if (collectionName is null) throw new ArgumentNullException(nameof(collectionName));
            settings = _settings.Value.Settings.FirstOrDefault(x => x.CollectionName == collectionName);

            if (settings is null) throw new MongoEntitySettingsDoesNotExistException(collectionName);            

            // Try to get from the created clients collection, otherwise creates a new client
            IMongoClient mongoClient = _mongoClients.TryGetValue(collectionName, out mongoClient) ? mongoClient : CreateClient(settings);

            return mongoClient;
        }

        private IMongoClient CreateClient(MongoEntitySettings databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.ConnectionString);

            _mongoClients.Add(databaseSettings.CollectionName, mongoClient);

            return mongoClient;
        }
    }
}
