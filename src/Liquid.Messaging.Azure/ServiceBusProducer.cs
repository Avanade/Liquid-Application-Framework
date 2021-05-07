using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Liquid.Messaging.Azure.Configuration;
using Liquid.Messaging.Azure.Parameters;
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
    /// <seealso cref="ILightProducer{TMessage}" />
    /// <seealso cref="IDisposable" />
    public class ServiceBusProducer<TMessage> : ILightProducer<TMessage>
    {
        private readonly ILogger _logger;
        private readonly ILightContextFactory _contextFactory;
        private readonly ServiceBusSettings _messagingSettings;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly ServiceBusProducerParameter _serviceBusProducerParameter;
        private TopicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The service bus configuration.</param>
        /// <param name="serviceBusProducerParameter">The service bus producer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException"></exception>
        public ServiceBusProducer(ILightContextFactory contextFactory,
                                     ILightTelemetryFactory telemetryFactory,
                                     ILoggerFactory loggerFactory,
                                     ILightMessagingConfiguration<ServiceBusSettings> messagingConfiguration,
                                     ServiceBusProducerParameter serviceBusProducerParameter)
        {
            _serviceBusProducerParameter = serviceBusProducerParameter;
            _contextFactory = contextFactory;
            _telemetryFactory = telemetryFactory;
            _messagingSettings = messagingConfiguration?.GetSettings(_serviceBusProducerParameter.ConnectionId) ?? 
                     throw new MessagingMissingConfigurationException(_serviceBusProducerParameter.ConnectionId);
            _logger = loggerFactory.CreateLogger(typeof(ServiceBusProducer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the Azure Service Bus Client.
        /// </summary>
        private void InitializeClient()
        {
            _client = new TopicClient(_messagingSettings.ConnectionString, _serviceBusProducerParameter.Topic);
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
                var telemetryKey = $"ServiceBusProducer_{_serviceBusProducerParameter.Topic}";
                
                telemetry.AddContext($"SendMessage_{nameof(TMessage)}");
                telemetry.StartTelemetryStopWatchMetric(telemetryKey);
                var aggregationId = context.GetAggregationId();
                var messageId = Guid.NewGuid().ToString();

                customHeaders.TryAdd("liquidCulture", context.ContextCulture);
                customHeaders.TryAdd("liquidChannel", context.ContextChannel);
                customHeaders.TryAdd("liquidCorrelationId", context.ContextId.ToString());
                customHeaders.TryAdd("liquidBusinessCorrelationId", context.BusinessContextId.ToString());

                var messageBytes = !_serviceBusProducerParameter.CompressMessage ? message.ToJsonBytes() : message.ToJson().GzipCompress();
                var messageRequest = new Message(messageBytes) { CorrelationId = aggregationId, MessageId = messageId };

                if (_serviceBusProducerParameter.CompressMessage)
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
                    compressed = _serviceBusProducerParameter.CompressMessage
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