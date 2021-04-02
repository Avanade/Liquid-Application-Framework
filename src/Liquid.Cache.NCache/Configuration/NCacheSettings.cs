using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Liquid.Cache.NCache.Configuration
{
    /// <summary>
    /// NCache Settings class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class NCacheSettings
    {
        /// <summary>
        /// Gets or sets the cache name.
        /// </summary>
        /// <value>
        /// The Cache name configuration.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the retry interval.
        /// </summary>
        /// <value>
        /// The retry interval.
        /// </value>
        [JsonProperty("retryInterval")]
        public int? RetryInterval { get; set; }

        /// <summary>
        /// Gets or sets the connection retries.
        /// </summary>
        /// <value>
        /// The connection retries.
        /// </value>
        [JsonProperty("connectionRetries")]
        public int? ConnectionRetries { get; set; }

        /// <summary>
        /// Gets or sets the enable keep alive.
        /// </summary>
        /// <value>
        /// The enable keep alive.
        /// </value>
        [JsonProperty("enableKeepAlive")]
        public bool? EnableKeepAlive { get; set; }

        /// <summary>
        /// Gets or sets the keep alive interval.
        /// </summary>
        /// <value>
        /// The keep alive interval.
        /// </value>
        [JsonProperty("keepAliveInterval")]
        public int? KeepAliveInterval { get; set; }

        /// <summary>
        /// Gets or sets the servers.
        /// </summary>
        /// <value>
        /// The servers.
        /// </value>
        [JsonProperty("servers")]
        public List<ServerData> Servers { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServerData
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        [JsonProperty("port")]
        public int? Port { get; set; }
    }
}
