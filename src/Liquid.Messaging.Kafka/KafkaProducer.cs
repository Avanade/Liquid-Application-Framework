using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.Kafka.Parameters;
using Liquid.Messaging.Kafka.Extensions;
using Microsoft.Extensions.Logging;
using Liquid.Messaging.Kafka.Configuration;

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
        private readonly ILightContextFactory _contextFactory;
        private readonly KafkaSettings _messagingSettings;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly KafkaProducerParameter _kafkaProducerParameter;
        private IProducer<Null, string> _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="kafkaProducerParameter">The kafka producer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException"></exception>
        public KafkaProducer(ILightContextFactory contextFactory,
                                ILightTelemetryFactory telemetryFactory,
                                ILoggerFactory loggerFactory,
                                ILightMessagingConfiguration<KafkaSettings> messagingConfiguration,
                                KafkaProducerParameter kafkaProducerParameter)
        {
            _kafkaProducerParameter = kafkaProducerParameter;
            _contextFactory = contextFactory;
            _telemetryFactory = telemetryFactory;
            _messagingSettings = messagingConfiguration?.GetSettings(_kafkaProducerParameter.ConnectionId) ??
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
            var telemetry = _telemetryFactory.GetTelemetry();
            if (customHeaders == null) customHeaders = new Dictionary<string, object>();

            try
            {
                var context = _contextFactory.GetContext();
                var telemetryKey = $"PubSubProducer_{_kafkaProducerParameter.Topic}";

                telemetry.AddContext($"SendMessage_{nameof(TMessage)}");
                telemetry.StartTelemetryStopWatchMetric(telemetryKey);
                var aggregationId = context.GetAggregationId();
                var messageId = Guid.NewGuid().ToString();

                customHeaders.TryAdd("liquidCulture", context.ContextCulture);
                customHeaders.TryAdd("liquidChannel", context.ContextChannel);
                customHeaders.TryAdd("liquidCorrelationId", context.ContextId.ToString());
                customHeaders.TryAdd("liquidBusinessCorrelationId", context.BusinessContextId.ToString());
                customHeaders.TryAdd("liquidAggregationId", aggregationId);
                customHeaders.TryAdd("liquidMessageId", messageId);
                if (_kafkaProducerParameter.CompressMessage) { customHeaders.TryAdd(CommonExtensions.ContentTypeHeader, CommonExtensions.GZipContentType); }


                var messageBody = !_kafkaProducerParameter.CompressMessage ? message.ToJson() : Encoding.UTF8.GetString(message.ToJson().GzipCompress());

                var request = new Message<Null, string> { Value = messageBody, Headers = new Headers().AddCustomHeaders(customHeaders) };

                await _client.ProduceAsync(_kafkaProducerParameter.Topic, request);
                
                telemetry.CollectTelemetryStopWatchMetric(telemetryKey, new
                {
                    producer = telemetryKey,
                    messageType = typeof(TMessage).FullName,
                    aggregationId,
                    messageId,
                    message,
                    headers = customHeaders,
                    compressed = _kafkaProducerParameter.CompressMessage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new MessagingProducerException(ex);
            }
            finally
            {
                telemetry.RemoveContext($"SendMessage_{nameof(TMessage)}");
            }
        }
    }
}