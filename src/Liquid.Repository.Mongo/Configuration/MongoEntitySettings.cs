using Liquid.Repository.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.Mongo.Configuration
{
    /// <summary>
    /// MongoDB repository data entity settings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MongoEntitySettings : IMongoEntitySettings
    {
        ///<inheritdoc/>
        public string CollectionName { get; set; }

        ///<inheritdoc/>
        public string ShardKey { get; set; }

        ///<inheritdoc/>
        public string DatabaseSettingsSectionName { get; set; }

        ///<inheritdoc/>
        public DatabaseSettings DatabaseSettings { get; set; }
    }
}
