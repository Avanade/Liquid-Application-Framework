using Liguid.Repository.Configuration;
using Liquid.Core.Configuration;
using Liquid.Repository.Mongo.Exceptions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Liquid.Repository.Mongo
{
    ///<inheritdoc/>
    public class MongoClientFactory : IMongoClientFactory
    {
        private readonly ILightConfiguration<List<LightConnectionSettings>> _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoClientFactory" /> class.
        /// </summary>
        /// <param name="configuration">Database configuration settings.</param>
        public MongoClientFactory(ILightConfiguration<List<LightConnectionSettings>> configuration)
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
