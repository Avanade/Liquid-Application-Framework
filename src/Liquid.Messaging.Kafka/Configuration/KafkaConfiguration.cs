using Liquid.Core.Configuration;
using Liquid.Messaging.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace Liquid.Messaging.Kafka.Configuration
{
    /// <summary>
    /// Kafka Configuration Class.
    /// </summary>
    /// <seealso cref="Liquid.Core.Configuration.LightConfiguration" />
    /// <seealso>
    /// <cref>
    ///     Liquid.Messaging.Configuration.ILightMessagingConfiguration{Liquid.Messaging.Kafka.Configuration.KafkaSettings}
    /// </cref>
    /// </seealso>
    public class KafkaConfiguration : LightConfiguration, ILightMessagingConfiguration<KafkaSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public KafkaConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public KafkaSettings Settings => throw new NotImplementedException(); //Not used.

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        /// <returns></returns>
        public KafkaSettings GetSettings(string configurationSection)
        {
            return GetConfigurationSection<KafkaSettings>($"liquid:messaging:kafka:{configurationSection}");
        }
    }

    /// <summary>
    /// Kafka Settings class.
    /// </summary>
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