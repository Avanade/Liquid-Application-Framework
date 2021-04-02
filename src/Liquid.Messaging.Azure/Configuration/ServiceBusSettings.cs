using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Liquid.Messaging.Azure.Configuration
{
    /// <summary>
    /// Azure Service Bus Custom Settings Class
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServiceBusSettings
    {
        /// <summary>
        /// Gets or sets the maximum concurrent calls.
        /// </summary>
        /// <value>
        /// The maximum concurrent calls.
        /// </value>
        [JsonProperty("maxConcurrentCalls")]
        public int MaxConcurrentCalls { get; set; }

        /// <summary>
        /// Gets or sets a value indicating to auto complete the received message.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic complete]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("autoComplete")]
        public bool AutoComplete { get; set; }
    }
}