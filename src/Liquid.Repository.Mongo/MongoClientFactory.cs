using Liguid.Repository.Configuration;
using Liquid.Core.Interfaces;
using Liquid.Repository.Configuration;
using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Exceptions;
using Microsoft.Extensions.Configuration;
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
        private readonly IOptionsSnapshot<DatabaseSettings> _allDatabaseConfigurations;

        private readonly IDictionary<string, IMongoClient> _mongoClients;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoClientFactory" /> class.
        /// </summary>
        /// <param name="databaseSettings">An IOptionsSnapshot with database settings.</param>
        public MongoClientFactory(IOptionsSnapshot<DatabaseSettings> databaseSettings)
        {
            if (databaseSettings is null) throw new LiquidDatabaseSettingsDoesNotExistException(nameof(databaseSettings));

            _allDatabaseConfigurations = databaseSettings;
            _mongoClients = new Dictionary<string, IMongoClient>();
        }

        ///<inheritdoc/>
        public IMongoClient GetClient(string databaseId)
        {
            if (databaseId is null) throw new ArgumentNullException(nameof(databaseId));

            // Try to get from the created clients collection, otherwise creates a new client
            IMongoClient mongoClient = _mongoClients.TryGetValue(databaseId, out mongoClient) ? mongoClient : CreateClient(databaseId);

            return mongoClient;
        }

        private IMongoClient CreateClient(string databaseId)
        {
            var databaseSettings = GetDatabaseSettings(databaseId);

            var mongoClient = new MongoClient(databaseSettings.ConnectionString);

            _mongoClients.Add(databaseId, mongoClient);

            return mongoClient;
        }

        private DatabaseSettings GetDatabaseSettings(string databaseId)
        {
            DatabaseSettings databaseSettings = null;

            if (_allDatabaseConfigurations != null)
                databaseSettings = _allDatabaseConfigurations.Get(databaseId);

            if (databaseSettings is null) 
                throw new LiquidDatabaseSettingsDoesNotExistException(databaseId);

            return databaseSettings;
        }
    }
}
