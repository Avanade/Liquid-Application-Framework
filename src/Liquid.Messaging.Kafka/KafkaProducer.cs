using Confluent.Kafka;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.Kafka.Configuration;
using Liquid.Messaging.Kafka.Extensions;
using Liquid.Messaging.Kafka.Parameters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Messaging.Kafka
{
    /// <summary>
    /// Google Cloud Pub/Sub Producer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the object.</typeparam>
    /// <seealso cref="ILightProducer{TMessage}" />
    /// <seealso cref="IDisposable" />
    public class KafkaProducer<TMessage> : ILightProducer<TMessage>
    {
        private readonly ILogger _logger;
        private readonly ILiquidContext _context;
        private readonly KafkaSettings _messagingSettings;
        private readonly KafkaProducerParameter _kafkaProducerParameter;
        private IProducer<Null, string> _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="context">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="kafkaProducerParameter">The kafka producer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException"></exception>
        public KafkaProducer(ILiquidContext context,
                                ILoggerFactory loggerFactory,
                                ILiquidConfiguration<KafkaSettings> messagingConfiguration,
                                KafkaProducerParameter kafkaProducerParameter)
        {
            _kafkaProducerParameter = kafkaProducerParameter;
            _context = context;
            _messagingSettings = messagingConfiguration?.Settings ??
                    throw new MessagingMissingConfigurationException(_kafkaProducerParameter.ConnectionId);
            _logger = loggerFactory.CreateLogger(typeof(KafkaProducer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the Azure Service Bus Client.
        /// </summary>
        private void InitializeClient()
        {
            //TODO: Review connection parameters with kafka.
            var config = new ProducerConfig
            {
                SocketKeepaliveEnable = _messagingSettings.SocketKeepAlive,
                SocketTimeoutMs = _messagingSettings.Timeout,
                BootstrapServers = _messagingSettings.ConnectionString,
                ClientId = _kafkaProducerParameter.ConnectionId
            };

            _client = new ProducerBuilder<Null, string>(config).Build();
        }

        /// <summary>
        /// Sends the message asynchronous.
        /// </summary>
        /// <param name="message">The message object.</param>
        /// <param name="customHeaders">The message custom headers.</param>
        /// <exception cref="ArgumentNullException">message</exception>
        /// <exception cref="MessagingProducerException"></exception>
        public async Task SendMessageAsync(TMessage message, IDictionary<string, object> customHeaders = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (customHeaders == null) customHeaders = new Dictionary<string, object>();

            try
            {
                var context = _context;
                var telemetryKey = $"PubSubProducer_{_kafkaProducerParameter.Topic}";

                var messageId = Guid.NewGuid().ToString();

                context.Upsert("MessageId", messageId);

                foreach (var item in context.current)
                {
                    customHeaders.TryAdd(item.Key, item.Value);
                }

                if (_kafkaProducerParameter.CompressMessage) { customHeaders.TryAdd(CommonExtensions.ContentTypeHeader, CommonExtensions.GZipContentType); }


                var messageBody = !_kafkaProducerParameter.CompressMessage ? message.ToJson() : Encoding.UTF8.GetString(message.ToJson().GzipCompress());

                var request = new Message<Null, string> { Value = messageBody, Headers = new Headers().AddCustomHeaders(customHeaders) };

                await _client.ProduceAsync(_kafkaProducerParameter.Topic, request);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new MessagingProducerException(ex);
            }
        }
    }
}