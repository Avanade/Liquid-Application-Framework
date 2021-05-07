using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Liquid.Messaging.RabbitMq.Configuration
{
    /// <summary>
    /// RabbitMq Custom Settings Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RabbitMqParameterSettings
    {
        /// <summary>
        /// Gets or sets the type of the exchange.
        /// </summary>
        /// <value>
        /// The type of the exchange.
        /// </value>
        [JsonProperty("exchangeType")]
        public string ExchangeType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Exchange/Queue[automatic delete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic delete]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("autoDelete")]
        public bool AutoDelete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Exchange/Queue is durable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if durable; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("durable")]
        public bool Durable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic ack].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic ack]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("autoAck")]
        public bool AutoAck { get; set; }

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        [JsonProperty("expiration")]
        public string Expiration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is persistent.
        /// </summary>
        /// <value>
        ///   <c>true</c> if persistent; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("persistent")]
        public bool Persistent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this Queue is exclusive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if exclusive; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("exclusive")]
        public bool Exclusive { get; set; }

        /// <summary>
        /// Gets or sets the exchange arguments.
        /// </summary>
        /// <value>
        /// The exchange arguments.
        /// </value>
        [JsonProperty("exchangeArguments")]
        public IDictionary<string,object> ExchangeArguments { get; set; }

        /// <summary>
        /// Gets or sets the queue arguments.
        /// </summary>
        /// <value>
        /// The queue arguments.
        /// </value>
        [JsonProperty("queueArguments")]
        public IDictionary<string,object> QueueArguments { get; set; }
    }
}