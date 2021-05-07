using Liquid.Core.Configuration;
using Liquid.Messaging.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace Liquid.Messaging.Gcp.Configuration
{
    /// <summary>
    /// Google Pub Sub Configuration Class.
    /// </summary>
    /// <seealso cref="Liquid.Core.Configuration.LightConfiguration" />
    /// <seealso>
    /// <cref>
    ///     Liquid.Messaging.Configuration.ILightMessagingConfiguration{Liquid.Messaging.Gcp.Configuration.PubSubSettings}
    /// </cref>
    /// </seealso>
    public class PubSubConfiguration : LightConfiguration, ILightMessagingConfiguration<PubSubSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PubSubConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public PubSubConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public PubSubSettings Settings => throw new NotImplementedException(); //Not used.

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public PubSubSettings GetSettings(string configurationSection)
        {
            return GetConfigurationSection<PubSubSettings>($"liquid:messaging:gcp:{configurationSection}");
        }
    }

    /// <summary>
    /// Google Pub Sub Settings.
    /// </summary>
    public class PubSubSettings
    {
        /// <summary>
        /// Gets or sets the connection string or file.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        [JsonProperty("connectionStringOrFile")]
        public string ConnectionStringOrFile { get; set; }

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        [JsonProperty("projectId")]
        public string ProjectId { get; set; }
    }
}
