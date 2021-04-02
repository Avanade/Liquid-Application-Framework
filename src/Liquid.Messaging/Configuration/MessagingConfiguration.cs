using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Liquid.Messaging.Configuration
{
    /// <summary>
    /// Event Configuration Class.
    /// </summary>
    /// <seealso>
    ///     <cref>Liquid.Configuration.AppSetting{Liquid.Messaging.Configuration.EventConfiguration}</cref>
    /// </seealso>
    [ExcludeFromCodeCoverage]
    [ConfigurationSection("messaging")]
    public class MessagingConfiguration : LightConfiguration, ILightConfiguration<List<MessagingSettings>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public MessagingConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public List<MessagingSettings> Settings => GetConfigurationSection<List<MessagingSettings>>();
    }

    /// <summary>
    /// Event Setting Configuration class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MessagingSettings
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        /// <value>
        /// The database connection string.
        /// </value>
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        [JsonProperty("parameters")]
        public List<CustomParameter> Parameters { get; set; }
    }

    /// <summary>
    /// Service parameter setting class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CustomParameter
    {
        /// <summary>
        /// Gets or sets the parameter key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("value")]
        public object Value { get; set; }
    }
}