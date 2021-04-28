using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Liquid.Messaging.Azure.Attributes;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Azure
{
    /// <summary>
    /// Azure Service Bus Producer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the object.</typeparam>
    /// <seealso cref="Liquid.Messaging.ILightProducer{TMessage}" />
    /// <seealso cref="System.IDisposable" />
    public abstract class ServiceBusProducer<TMessage> : ILightProducer<TMessage>
    {
        private readonly ILogger _logger;
        private readonly ILightContextFactory _contextFactory;
        private readonly List<MessagingSettings> _messagingSettings;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly ServiceBusProducerAttribute _attribute;
        private TopicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        /// <exception cref="NotImplementedException">The {nameof(ServiceBusProducerAttribute)} attribute decorator must be added to configuration class.</exception>
        protected ServiceBusProducer(ILightContextFactory contextFactory,
                                      ILightTelemetryFactory telemetryFactory,
                                      ILoggerFactory loggerFactory,
                                      ILightConfiguration<List<MessagingSettings>> messagingSettings)
        {
            if (!GetType().GetCustomAttributes(typeof(ServiceBusProducerAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(ServiceBusProducerAttribute)} attribute decorator must be added to configuration class.");
            }
            _attribute = GetType().GetCustomAttribute<ServiceBusProducerAttribute>(true);
            _contextFactory = contextFactory;
            _telemetryFactory = telemetryFactory;
            _messagingSettings = messagingSettings?.Settings ?? throw new MessagingMissingConfigurationException("messaging");
            _logger = loggerFactory.CreateLogger(typeof(ServiceBusProducer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the Azure Service Bus Client.
        /// </summary>
        private void InitializeClient()
        {
            var messagingSettings = _messagingSettings.GetMessagingSettings(_attribute.ConnectionId);
            _client = new TopicClient(messagingSettings.ConnectionString, _attribute.Topic);
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
            if (customHeaders == null) customHeaders = new Dictionary<string, object>();

            try
            {
                var context = _contextFactory.GetContext();
                var telemetryKey = $"ServiceBusProducer_{_attribute.Topic}";
                
                telemetry.AddContext($"SendMessage_{nameof(TMessage)}");
                telemetry.StartTelemetryStopWatchMetric(telemetryKey);
                var aggregationId = context.GetAggregationId();
                var messageId = Guid.NewGuid().ToString();

                customHeaders.TryAdd("liquidCulture", context.ContextCulture);
                customHeaders.TryAdd("liquidChannel", context.ContextChannel);
                customHeaders.TryAdd("liquidCorrelationId", context.ContextId.ToString());
                customHeaders.TryAdd("liquidBusinessCorrelationId", context.BusinessContextId.ToString());

                var messageBytes = !_attribute.CompressMessage ? message.ToJsonBytes() : message.ToJson().GzipCompress();
                var messageRequest = new Message(messageBytes) { CorrelationId = aggregationId, MessageId = messageId };

                if (_attribute.CompressMessage)
                {
                    messageRequest.ContentType = CommonExtensions.GZipContentType;
                }

                messageRequest.UserProperties.AddRange(customHeaders);

                await _client.SendAsync(messageRequest);

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