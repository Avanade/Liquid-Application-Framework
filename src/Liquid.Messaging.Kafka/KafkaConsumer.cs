using Confluent.Kafka;
using Liquid.Core.Entities;
using Liquid.Core.Exceptions;
using Liquid.Core.Interfaces;
using Liquid.Messaging.Kafka.Extensions;
using Liquid.Messaging.Kafka.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
        public Task RegisterMessageHandler(CancellationToken cancellationToken = default)
        {
            if (ConsumeMessageAsync is null)
            {
                throw new NotImplementedException($"The {nameof(ProcessErrorAsync)} action must be added to class.");
            }

            ProcessErrorAsync += ProcessError;

            _client = _factory.GetConsumer(_settings);


            var task = Task.Run( async () =>
            {
                using (_client)
                {
                    _client.Subscribe(_settings.Topic);

                    await MessageHandler(cancellationToken);                    
                }
            });

            return task;
        }

        /// <summary>
        /// Process incoming messages.
        /// </summary>
        /// <param name="cancellationToken"> Propagates notification that operations should be canceled.</param>
        protected async Task MessageHandler(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var deliverEvent = _client.Consume(cancellationToken);

                    _ = Task.Run(async () =>
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
                            var errorArgs = new ConsumerErrorEventArgs
                            {
                                Exception = ex,
                            };

                            await ProcessErrorAsync(errorArgs);
                        }
                    });                    
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

        private Task ProcessError(ConsumerErrorEventArgs args)
        {
            _client.Close();
            throw args.Exception;
        }
    }
}