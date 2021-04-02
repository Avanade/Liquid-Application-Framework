using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Liquid.Messaging.Aws.Attributes;
using Liquid.Messaging.Aws.Extensions;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Microsoft.Extensions.Logging;
using MessageAttributeValue = Amazon.SimpleNotificationService.Model.MessageAttributeValue;

namespace Liquid.Messaging.Aws
{
    /// <summary>
    /// AWS SNS Producer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    /// <seealso cref="ILightProducer{TMessage}" />
    public abstract class SnsProducer<TMessage> : ILightProducer<TMessage>
    {
        private readonly ILogger _logger;
        private readonly ILightContextFactory _contextFactory;
        private readonly MessagingSettings _messagingSettings;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly SnsProducerAttribute _attribute;
        private AmazonSimpleNotificationServiceClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqsProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        /// <exception cref="System.NotImplementedException">The {nameof(SnsProducerAttribute)} attribute decorator must be added to configuration class.</exception>
        protected SnsProducer(ILightContextFactory contextFactory,
                              ILightTelemetryFactory telemetryFactory,
                              ILoggerFactory loggerFactory,
                              ILightConfiguration<List<MessagingSettings>> messagingSettings)
        {
            if (!GetType().GetCustomAttributes(typeof(SnsProducerAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(SnsProducerAttribute)} attribute decorator must be added to configuration class.");
            }
            _attribute = GetType().GetCustomAttribute<SnsProducerAttribute>(true);
            _contextFactory = contextFactory;
            _telemetryFactory = telemetryFactory;
            _messagingSettings = messagingSettings?.Settings?.GetMessagingSettings(_attribute.ConnectionId) ?? throw new MessagingMissingConfigurationException("messaging");
            _logger = loggerFactory.CreateLogger(typeof(SqsProducer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the client.
        /// </summary>
        private void InitializeClient()
        {
            var credentials = new BasicAWSCredentials(_messagingSettings.GetAwsAccessKey(), _messagingSettings.GetAwsSecretKey());
            var config = new AmazonSimpleNotificationServiceConfig()
            {
                ServiceURL = _messagingSettings.ConnectionString,
                RegionEndpoint = _messagingSettings.GetAwsRegion()
            };

            _client = new AmazonSimpleNotificationServiceClient(credentials, config);

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
                var telemetryKey = $"SnsProducer_{_attribute.Topic}";

                telemetry.AddContext($"SendMessage_{nameof(TMessage)}");
                telemetry.StartTelemetryStopWatchMetric(telemetryKey);
                var aggregationId = context.GetAggregationId();

                customHeaders.TryAdd("liquidCulture", context.ContextCulture);
                customHeaders.TryAdd("liquidChannel", context.ContextChannel);
                customHeaders.TryAdd("liquidCorrelationId", context.ContextId.ToString());
                customHeaders.TryAdd("liquidBusinessCorrelationId", context.BusinessContextId.ToString());
                customHeaders.TryAdd("liquidAggregationId", aggregationId);

                var messageBody = message.ToString();
                if (_attribute.MessageStructure.Equals("json", StringComparison.InvariantCultureIgnoreCase))
                {
                    messageBody = !_attribute.CompressMessage ? message.ToJson() : Convert.ToBase64String(message.ToJson().GzipCompress());
                }

                if (_attribute.CompressMessage) { customHeaders.TryAdd(CommonExtensions.ContentTypeHeader, CommonExtensions.GZipContentType); }

                var topicResponse = await _client.CreateTopicAsync(new CreateTopicRequest(_attribute.Topic));

                var request = new PublishRequest
                {
                    TopicArn = topicResponse.TopicArn,
                    Message = messageBody,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>().AddCustomHeaders(customHeaders)
                };

                if (customHeaders.ContainsKey("PhoneNumber")) { request.PhoneNumber = customHeaders["PhoneNumber"].ToString(); }
                if (customHeaders.ContainsKey("Subject")) { request.Subject = customHeaders["Subject"].ToString(); }
                
                await _client.PublishAsync(request);

                telemetry.CollectTelemetryStopWatchMetric(telemetryKey, new
                {
                    producer = telemetryKey,
                    messageType = typeof(TMessage).FullName,
                    aggregationId,
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