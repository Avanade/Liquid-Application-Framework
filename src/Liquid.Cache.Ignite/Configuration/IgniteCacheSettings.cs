using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Liquid.Cache.Ignite.Configuration
{
    /// <summary>
    /// Apache Ignite Settings class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IgniteCacheSettings
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
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the socket timeout.
        /// </summary>
        /// <value>
        /// The socket timeout.
        /// </value>
        [JsonProperty("socketTimeout")]
        public int? SocketTimeout { get; set; }

        /// <summary>
        /// Gets or sets the enable partition awareness.
        /// </summary>
        /// <value>
        /// The enable partition awareness.
        /// </value>
        [JsonProperty("enablePartitionAwareness")]
        public bool? EnablePartitionAwareness { get; set; }

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
    }
}
