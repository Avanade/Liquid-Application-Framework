using Liquid.Core.Configuration;
using Liquid.Messaging.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace Liquid.Messaging.RabbitMq.Configuration
{
    /// <summary>
    /// RabbitMq Configuration Class.
    /// </summary>
    /// <seealso cref="Liquid.Core.Configuration.LightConfiguration" />
    /// /// <seealso>
    /// <cref>
    ///     Liquid.Messaging.Configuration.ILightMessagingConfiguration{Liquid.Messaging.RabbitMq.Configuration.RabbitMqSettings}
    /// </cref>
    /// </seealso>
    public class RabbitMqConfiguration : LightConfiguration, ILightMessagingConfiguration<RabbitMqSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public RabbitMqConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public RabbitMqSettings Settings => throw new NotImplementedException(); // Not Used.

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        /// <returns></returns>
        public RabbitMqSettings GetSettings(string configurationSection)
        {
            return GetConfigurationSection<RabbitMqSettings>($"liquid:messaging:rabbitMq:{configurationSection}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RabbitMqSettings
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
        /// Gets or sets the request heart beat in seconds.
        /// </summary>
        /// <value>
        /// The request heart beat in seconds.
        /// </value>
        [JsonProperty("requestHeartBeatInSeconds")]
        public ushort? RequestHeartBeatInSeconds { get; set; }

        /// <summary>
        /// Gets or sets the prefetch count.
        /// </summary>
        /// <value>
        /// The prefetch count.
        /// </value>
        [JsonProperty("prefetchCount")]
        public ushort? PrefetchCount { get; set; }

        /// <summary>
        /// Gets or sets the prefetch.
        /// </summary>
        /// <value>
        /// The prefetch.
        /// </value>
        [JsonProperty("prefetch")]
        public uint? Prefetch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Qos is global.
        /// </summary>
        /// <value>
        ///   <c>true</c> if global; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("global")]
        public bool? Global { get; set; }

        /// <summary>
        /// Gets or sets the auto recovery property of Rabbit to recover exchanges and queue bindings.
        /// </summary>
        /// <value>
        /// The auto recovery value.
        /// </value>
        [JsonProperty("autoRecovery")]
        public bool? AutoRecovery { get; set; }
    }
}
