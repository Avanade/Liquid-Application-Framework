using Liquid.Core.Attributes;
using Newtonsoft.Json;

namespace Liquid.Messaging.Kafka.Configuration
{
    /// <summary>
    /// Kafka Settings class.
    /// </summary>
    [LiquidSectionName("liquid:messaging:kafka:")]
    public class KafkaSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        [JsonProperty("socketKeepalive")]
        public bool SocketKeepAlive { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        [JsonProperty("timeout")]
        public int Timeout { get; set; }
    }
}