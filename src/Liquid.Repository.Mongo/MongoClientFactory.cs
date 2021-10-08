using Liguid.Repository.Configuration;
using Liquid.Core.Interfaces;
using Liquid.Repository.Configuration;
using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Exceptions;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.Repository.Mongo
{
    ///<inheritdoc/>
    public class MongoClientFactory : IMongoClientFactory
    {
        [Obsolete("This private member is deprecated along with the constructor that injects it.", false)]
        private readonly ILiquidConfiguration<MongoSettings> _configuration;

        private readonly IDictionary<string, DatabaseSettings> _databaseConfigurations;

        private readonly IDictionary<string, IMongoClient> _mongoClients;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoClientFactory" /> class.
        /// </summary>
        /// <param name="configuration">Database configuration settings.</param>
        [Obsolete("This constructor is deprecated. Use MongoClientFactory(IConfiguration configurationSection) instead.", false)]
        public MongoClientFactory(ILiquidConfiguration<MongoSettings> configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mongoClients = new Dictionary<string, IMongoClient>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoClientFactory" /> class.
        /// </summary>
        /// <param name="configurationSection">A configuration section with all database settings.</param>
        public MongoClientFactory(IConfiguration configurationSection)
        {
            if (configurationSection is null || 
                !configurationSection.GetChildren().Any()) throw new LiquidDatabaseSettingsDoesNotExistException(nameof(configurationSection));

            _databaseConfigurations = new Dictionary<string, DatabaseSettings>();
            _mongoClients = new Dictionary<string, IMongoClient>();

            foreach (var databaseSettings in configurationSection.GetChildren())
            {
                _databaseConfigurations.Add(databaseSettings.Key, databaseSettings.Get<DatabaseSettings>());
            }
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

        // TODO: refactor or remove this method and the deprecated items on the next major release
        private DatabaseSettings GetDatabaseSettings(string databaseId)
        {
            DatabaseSettings databaseSettings = null;

            if (_configuration != null)
                databaseSettings = _configuration.Settings.GetDbSettings(databaseId);

            if (_databaseConfigurations != null)
                _databaseConfigurations.TryGetValue(databaseId, out databaseSettings);

            if (databaseSettings is null) 
                throw new LiquidDatabaseSettingsDoesNotExistException(databaseId);

            return databaseSettings;
        }
    }
}
