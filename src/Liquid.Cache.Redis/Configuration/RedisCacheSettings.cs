using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Cache.Redis.Configuration
{
    /// <summary>
    /// Redis Cache Settings class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RedisCacheSettings
    {
        /// <summary>
        /// Gets or sets the server configuration.
        /// </summary>
        /// <value>
        /// The server configuration.
        /// </value>
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }
    }
}
