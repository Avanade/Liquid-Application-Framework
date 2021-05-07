using Liquid.Core.Configuration;
using Liquid.Messaging.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Messaging.Azure.Configuration
{
    /// <summary>
    /// Service Bus Configuration Class.
    /// </summary>
    /// <seealso cref="Liquid.Core.Configuration.LightConfiguration" />
    /// <seealso>
    /// <cref>
    ///     Liquid.Messaging.Configuration.ILightMessagingConfiguration{Liquid.Messaging.Azure.Configuration.ServiceBusSettings}
    /// </cref>
    /// </seealso>
    public class ServiceBusConfiguration : LightConfiguration, ILightMessagingConfiguration<ServiceBusSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ServiceBusConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public ServiceBusSettings Settings => throw new NotImplementedException(); //Not used.

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="configurationSection">The configuration section.</param>
        /// <returns></returns>
        public ServiceBusSettings GetSettings(string configurationSection)
        {
            return GetConfigurationSection<ServiceBusSettings>($"liquid:messaging:azure:{configurationSection}");
        }
    }

    /// <summary>
    /// Azure Service Bus Custom Settings Class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServiceBusSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }        
    }
}
