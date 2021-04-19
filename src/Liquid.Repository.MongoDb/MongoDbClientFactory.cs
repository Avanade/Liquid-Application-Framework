using Liguid.Repository.Configuration;
using Liquid.Core.Configuration;
using Liquid.Repository.MongoDb.Exceptions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Liquid.Repository.MongoDb
{
    ///<inheritdoc/>
    public class MongoDbClientFactory : IMongoDbClientFactory
    {
        private readonly ILightConfiguration<List<LightConnectionSettings>> _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbClientFactory" /> class.
        /// </summary>
        /// <param name="configuration">Database configuration settings.</param>
        public MongoDbClientFactory(ILightConfiguration<List<LightConnectionSettings>> configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        ///<inheritdoc/>
        public IMongoClient GetClient(string connectionId)
        {
            var databaseSettings = _configuration?.Settings?.GetConnectionSetting(connectionId);
            if (databaseSettings == null) throw new LightDatabaseConfigurationDoesNotExistException(connectionId);


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
