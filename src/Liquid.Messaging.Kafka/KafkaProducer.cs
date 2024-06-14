using Confluent.Kafka;
using Liquid.Core.Utils;
using Liquid.Core.Extensions;
using Liquid.Core.Exceptions;
using Liquid.Core.Interfaces;
using Liquid.Messaging.Kafka.Extensions;
using Liquid.Messaging.Kafka.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Messaging.Kafka
{
    ///<inheritdoc/>
    public class KafkaProducer<TEntity> : ILiquidProducer<TEntity>
    {
        private readonly IProducer<Null, string> _client;
        private readonly KafkaSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaProducer{TEntity}"/> class.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="factory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public KafkaProducer(KafkaSettings settings, IKafkaFactory factory)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _settings = settings;

            _client = factory.GetProducer(settings);
        }

        ///<inheritdoc/>
        public async Task SendMessagesAsync(IEnumerable<TEntity> messageBodies)
        {
            try
            {
                foreach (var message in messageBodies)
                {
                    await SendMessageAsync(message);
                }
            }
            catch (Exception ex)
            {
                throw new MessagingProducerException(ex);
            }
        }

        ///<inheritdoc/>
        public async Task SendMessageAsync(TEntity messageBody, IDictionary<string, object> customProperties = null)
        {            
            try
            {
                var message = GetMessage(messageBody, customProperties);

                await _client.ProduceAsync(_settings.Topic, message);
            }
            catch (Exception ex)
            {
                throw new MessagingProducerException(ex);
            }
        }

        private Message<Null, string> GetMessage(TEntity messageBody, IDictionary<string, object> customProperties)
        {
            var message = !_settings.CompressMessage ? messageBody.ToJsonString() : Encoding.UTF8.GetString(messageBody.ToJsonString().GzipCompress());

            var request = new Message<Null, string>
            {
                Value = message,
                Headers = customProperties is null ? null : new Headers().AddCustomHeaders(customProperties)
            };
            return request;
        }
    }
}
