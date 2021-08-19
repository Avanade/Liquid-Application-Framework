using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Messaging.Azure.Configuration;
using Liquid.Messaging.Azure.Parameters;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private readonly ILiquidContext _context;
        private readonly ServiceBusSettings _messagingSettings;
        private readonly ServiceBusProducerParameter _serviceBusProducerParameter;
        private TopicClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The service bus configuration.</param>
        /// <param name="serviceBusProducerParameter">The service bus producer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException"></exception>
        public ServiceBusProducer(ILiquidContext contextFactory,
                                     ILoggerFactory loggerFactory,
                                     ILiquidConfiguration<ServiceBusSettings> messagingConfiguration,
                                     ServiceBusProducerParameter serviceBusProducerParameter)
        {
            _serviceBusProducerParameter = serviceBusProducerParameter;
            _context = contextFactory;
            _messagingSettings = messagingConfiguration?.Settings ??
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

            if (customHeaders == null) customHeaders = new Dictionary<string, object>();

            try
            {
                var context = _context;
                var telemetryKey = $"ServiceBusProducer_{_serviceBusProducerParameter.Topic}";


                var aggregationId = context.Get("AggregationId").ToString();
                var messageId = Guid.NewGuid().ToString();

                customHeaders.TryAdd("Culture", context.Get("Culture"));
                customHeaders.TryAdd("Channel", context.Get("Channel"));
                customHeaders.TryAdd("CorrelationId", context.Get("CorrelationId"));
                customHeaders.TryAdd("BusinessCorrelationId", context.Get("BusinessCorrelationId"));

                var messageBytes = !_serviceBusProducerParameter.CompressMessage ? message.ToJsonBytes() : message.ToJson().GzipCompress();
                var messageRequest = new Message(messageBytes) { CorrelationId = aggregationId, MessageId = messageId };

                if (_serviceBusProducerParameter.CompressMessage)
                {
                    messageRequest.ContentType = CommonExtensions.GZipContentType;
                }

                messageRequest.UserProperties.AddRange(customHeaders);

                await _client.SendAsync(messageRequest);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new MessagingProducerException(ex);
            }
        }
    }
}