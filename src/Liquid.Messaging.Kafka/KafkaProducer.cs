using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.Kafka.Attributes;
using Liquid.Messaging.Kafka.Extensions;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Kafka
{
    /// <summary>
    /// Google Cloud Pub/Sub Producer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the object.</typeparam>
    /// <seealso cref="Liquid.Messaging.ILightProducer{TMessage}" />
    /// <seealso cref="System.IDisposable" />
    public abstract class KafkaProducer<TMessage> : ILightProducer<TMessage>
    {
        private readonly ILogger _logger;
        private readonly ILightContextFactory _contextFactory;
        private readonly MessagingSettings _messagingSettings;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly KafkaProducerAttribute _attribute;
        private IProducer<Null, string> _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        /// <exception cref="NotImplementedException">The {nameof(PubSubProducerAttribute)} attribute decorator must be added to configuration class.</exception>
        protected KafkaProducer(ILightContextFactory contextFactory,
                                 ILightTelemetryFactory telemetryFactory,
                                 ILoggerFactory loggerFactory,
                                 ILightConfiguration<List<MessagingSettings>> messagingSettings)
        {
            if (!GetType().GetCustomAttributes(typeof(KafkaProducerAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(KafkaProducerAttribute)} attribute decorator must be added to configuration class.");
            }
            _attribute = GetType().GetCustomAttribute<KafkaProducerAttribute>(true);
            _contextFactory = contextFactory;
            _telemetryFactory = telemetryFactory;
            _messagingSettings = messagingSettings?.Settings?.GetMessagingSettings(_attribute.ConnectionId) ?? throw new MessagingMissingConfigurationException("messaging");
            _logger = loggerFactory.CreateLogger(typeof(KafkaProducer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the Azure Service Bus Client.
        /// </summary>
        private void InitializeClient()
        {
            //TODO: Rever forma de conexão e testar conectividade com o Kafka.
            var config = new ProducerConfig
            {
                SocketKeepaliveEnable = _messagingSettings.GetSocketKeepAlive(),
                SocketTimeoutMs = _messagingSettings.GetTimeout(),
                BootstrapServers = _messagingSettings.ConnectionString,
                ClientId = _messagingSettings.Id
            };

            _client = new ProducerBuilder<Null, string>(config).Build();
        }

        /// <summary>
        /// Sends the message asynchronous.
        /// </summary>
        /// <param name="message">The message object.</param>
        /// <param name="customHeaders">The message custom headers.</param>
        /// <exception cref="System.ArgumentNullException">message</exception>
        /// <exception cref="MessagingProducerException"></exception>
        public async Task SendMessageAsync(TMessage message, IDictionary<string, object> customHeaders = null)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            var telemetry = _telemetryFactory.GetTelemetry();

            try
            {
                var context = _contextFactory.GetContext();
                var telemetryKey = $"PubSubProducer_{_attribute.Topic}";

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
                if (_attribute.CompressMessage) { customHeaders.TryAdd(CommonExtensions.ContentTypeHeader, CommonExtensions.GZipContentType); }


                var messageBody = !_attribute.CompressMessage ? message.ToJson() : Encoding.UTF8.GetString(message.ToJson().GzipCompress());

                var request = new Message<Null, string> { Value = messageBody, Headers = new Headers().AddCustomHeaders(customHeaders) };

                await _client.ProduceAsync(_attribute.Topic, request);
                
                telemetry.CollectTelemetryStopWatchMetric(telemetryKey, new
                {
                    producer = telemetryKey,
                    messageType = typeof(TMessage).FullName,
                    aggregationId,
                    messageId,
                    message,
                    headers = customHeaders,
                    compressed = _attribute.CompressMessage
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