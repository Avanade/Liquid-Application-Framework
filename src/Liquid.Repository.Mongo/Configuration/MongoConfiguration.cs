using Liquid.Core.Configuration;
using Liquid.Repository.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Repository.Mongo.Configuration
{
    /// <summary>
    /// Mongo connection configuration
    /// </summary>
    /// <seealso cref="Liquid.Core.Configuration.LightConfiguration" />
    /// <seealso cref="Liquid.Repository.Configuration.ILightDatabaseConfiguration{Liquid.Repository.Mongo.Configuration.MongoSettings}" />
    public class MongoConfiguration : LightConfiguration, ILightDatabaseConfiguration<MongoSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MongoConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <inheritdoc/>
        public MongoSettings Settings => throw new NotImplementedException(); // Not used

        /// <inheritdoc/>
        public MongoSettings GetSettings(string configurationSection)
        {
            return GetConfigurationSection<MongoSettings>($"liquid:databases:mongo:{configurationSection}");
        }
    }

    /// <summary>
    /// Mongo settings class.
    /// </summary>
    public class MongoSettings
    {
        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        /// <value>
        /// The database connection string.
        /// </value>
        [JsonProperty(PropertyName = "connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>
        /// The name of the database.
        /// </value>
        [JsonProperty(PropertyName = "databaseName")]
        public string DatabaseName { get; set; }
    }
}
