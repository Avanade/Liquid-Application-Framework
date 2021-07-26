using Liquid.Core.Attributes;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Messaging.Azure.Configuration
{
    /// <summary>
    /// Azure Service Bus Custom Settings Class
    /// </summary>
    [ExcludeFromCodeCoverage]
    [LiquidSectionName("liquid:messaging:azure")]
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
