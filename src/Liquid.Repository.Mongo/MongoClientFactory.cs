using Liguid.Repository.Configuration;
using Liquid.Core.Interfaces;
using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Exceptions;
using MongoDB.Driver;
using System;

namespace Liquid.Repository.Mongo
{
    ///<inheritdoc/>
    public class MongoClientFactory : IMongoClientFactory
    {
        private readonly ILiquidConfiguration<MongoSettings> _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoClientFactory" /> class.
        /// </summary>
        /// <param name="configuration">Database configuration settings.</param>
        public MongoClientFactory(ILiquidConfiguration<MongoSettings> configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        ///<inheritdoc/>
        public IMongoClient GetClient(string databaseName)
        {
            var databaseSettings = _configuration.Settings.GetDbSettings(databaseName);
            if (databaseSettings == null) throw new LightDatabaseConfigurationDoesNotExistException(databaseName);

            var mongoClient = new MongoClient(databaseSettings.ConnectionString);

            var database = mongoClient.GetDatabase(databaseSettings.DatabaseName);

            if (database is null)
            {
                throw new MongoDatabaseDoesNotExistException(databaseSettings.DatabaseName);
            }

            return mongoClient;
        }
    }
}
