using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Liquid.Messaging.Kafka.Tests.Messages
{
    /// <summary>
    /// Test Message Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class KafkaTestMessage
    {
        /// <summary>
        /// Gets or sets the self.
        /// </summary>
        /// <value>
        /// The self.
        /// </value>
        [JsonIgnore]
        public static KafkaTestMessage Self { get; set; }

        /// <summary>
        /// Gets or sets the test message identifier.
        /// </summary>
        /// <value>
        /// The test message identifier.
        /// </value>
        [JsonProperty("testMessageId")]
        public int TestMessageId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}