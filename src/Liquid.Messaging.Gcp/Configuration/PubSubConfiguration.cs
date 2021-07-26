using Liquid.Core.Attributes;
using Newtonsoft.Json;

namespace Liquid.Messaging.Gcp.Configuration
{
    /// <summary>
    /// Google Pub Sub Settings.
    /// </summary>
    /// 
    [LiquidSectionName("liquid:messaging:gcp:")]
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
