using Liguid.Repository.Configuration;
using Liquid.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Liquid.Repository.EntityFramework
{
    ///<inheritdoc/>
    public class EntityFrameworkClientFactory : IEntityFrameworkClientFactory
    {
        private readonly ILightConfiguration<List<LightConnectionSettings>> _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkClientFactory"/> class.
        /// </summary>
        /// <param name="configuration"> Databases connection settings.</param>
        public EntityFrameworkClientFactory(ILightConfiguration<List<LightConnectionSettings>> configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        ///<inheritdoc/>
        public DbContext GetClient(string connectionId)
        {
            var databaseSettings = _configuration?.Settings?.GetConnectionSetting(connectionId);
            if (databaseSettings == null) 
                throw new LightDatabaseConfigurationDoesNotExistException(connectionId);
                       

            var dbContext = new ApplicationDbContext(databaseSettings.ConnectionString);

            if (dbContext?.Database is null)
            {
                throw new DatabaseDoesNotExistException(databaseSettings.DatabaseName);
            }

            return dbContext;
        }
    }
}
