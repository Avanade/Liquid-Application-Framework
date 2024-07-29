using Confluent.Kafka;
using Liquid.Core.Extensions;
using Liquid.Core.Utils;
using Liquid.Core.Exceptions;
using Liquid.Core.Interfaces;
using Liquid.Messaging.Kafka.Extensions;
using Liquid.Messaging.Kafka.Settings;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Liquid.Core.Entities;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Liquid.Messaging.Kafka
{
    ///<inheritdoc/>
    public class KafkaConsumer<TEntity> : ILiquidConsumer<TEntity>
    {
        private readonly IKafkaFactory _factory;
        private readonly KafkaSettings _settings;
        private IConsumer<Ignore, string> _client;


        ///<inheritdoc/>
        public event Func<ConsumerMessageEventArgs<TEntity>, CancellationToken, Task> ConsumeMessageAsync;

        ///<inheritdoc/>
        public event Func<ConsumerErrorEventArgs, Task> ProcessErrorAsync;

        /// <summary>
        /// Initialize a new instance of <see cref="KafkaConsumer{TEntity}"/>
        /// </summary>
        /// <param name="kafkaFactory">Kafka client provider service.</param>
        /// <param name="kafkaSettings">Configuration properties set.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public KafkaConsumer(IKafkaFactory kafkaFactory, IOptions<KafkaSettings> kafkaSettings)
        {
            _factory = kafkaFactory ?? throw new ArgumentNullException(nameof(kafkaFactory));
            _settings = kafkaSettings?.Value ?? throw new ArgumentNullException(nameof(kafkaSettings));
        }

        ///<inheritdoc/>
        public void RegisterMessageHandler(CancellationToken cancellationToken = default)
        {
            if (ConsumeMessageAsync is null)
            {
                throw new NotImplementedException($"The {nameof(ProcessErrorAsync)} action must be added to class.");
            }

            _client = _factory.GetConsumer(_settings);

            using (var consumer = _client)
            {
                consumer.Subscribe(_settings.Topic);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var result = _client.Consume();

                        MessageHandler(result, cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }

        }

        /// <summary>
        /// Process incoming messages.
        /// </summary>
        /// <param name="deliverEvent">Message to be processed.</param>
        /// <param name="cancellationToken"> Propagates notification that operations should be canceled.</param>
        protected async Task MessageHandler(ConsumeResult<Ignore, string> deliverEvent, CancellationToken cancellationToken)
        {
            try
            {
                await ConsumeMessageAsync(GetEventArgs(deliverEvent), cancellationToken);

                if (!_settings.EnableAutoCommit)
                {
                    _client.Commit(deliverEvent);
                }
            }
            catch (Exception ex)
            {
                throw new MessagingConsumerException(ex);
            }
        }

        private ConsumerMessageEventArgs<TEntity> GetEventArgs(ConsumeResult<Ignore, string> deliverEvent)
        {
            var headers = deliverEvent.Message.Headers.GetCustomHeaders();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<TEntity>(deliverEvent.Message.Value, options);

            return new ConsumerMessageEventArgs<TEntity> { Data = data, Headers = headers };
        }
    }
}