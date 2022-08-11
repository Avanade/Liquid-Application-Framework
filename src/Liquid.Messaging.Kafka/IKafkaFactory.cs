using Confluent.Kafka;
using Liquid.Messaging.Kafka.Settings;
using System;

namespace Liquid.Messaging.Kafka
{
    /// <summary>
    /// Provides new instances if Kafka <see cref="IConsumer{TKey, TValue}"/> and <see cref="IProducer{TKey, TValue}"/>.
    /// </summary>
    public interface IKafkaFactory
    {
        /// <summary>
        /// Create a new instance of a high-level Apache Kafka consumer with deserialization capability.
        /// </summary>
        /// <param name="settings">Consumer configuration set.</param>
        IConsumer<Ignore, string> GetConsumer(KafkaSettings settings);

        /// <summary>
        /// Create a neu instance of a high level producer with serialization capability.
        /// </summary>
        /// <param name="settings">Consumer configuration set.</param>
        IProducer<Null, string> GetProducer(KafkaSettings settings);
    }
}
