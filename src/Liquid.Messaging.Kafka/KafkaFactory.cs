using Confluent.Kafka;
using Liquid.Core.Exceptions;
using Liquid.Messaging.Kafka.Settings;
using System;

namespace Liquid.Messaging.Kafka
{
    ///<inheritdoc/>
    public class KafkaFactory : IKafkaFactory
    {
        ///<inheritdoc/>
        public IConsumer<Ignore, string> GetConsumer(KafkaSettings settings)
        {  
            try
            {
                var config = MapConsumerSettings(settings);

                var client = new ConsumerBuilder<Ignore, string>(config).Build();

                return client;
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex, "for topic '" + settings?.Topic + "'");
            }
        }

        ///<inheritdoc/>
        public IProducer<Null, string> GetProducer(KafkaSettings settings)
        {
            try
            {
                var config = MapProducerSettings(settings);

                return new ProducerBuilder<Null, string>(config).Build();
            }
            catch(Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex, "for topic '" + settings?.Topic + "'");
            }
        }

        private static ConsumerConfig MapConsumerSettings(KafkaSettings settings)
        {
            if(settings == null)
                throw new ArgumentNullException(nameof(settings));

            return new ConsumerConfig
            {
                SocketKeepaliveEnable = settings.SocketKeepAlive,
                SocketTimeoutMs = settings.Timeout,
                BootstrapServers = settings.ConnectionString,
                ClientId = settings.ConnectionId,
                EnableAutoCommit = settings.EnableAutoCommit,
            };
        }

        private static ProducerConfig MapProducerSettings(KafkaSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            return new ProducerConfig
            {
                SocketKeepaliveEnable = settings.SocketKeepAlive,
                SocketTimeoutMs = settings.Timeout,
                BootstrapServers = settings.ConnectionString,
                ClientId = settings.ConnectionId,
            };
        }


    }
}
