using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Liquid.Messaging.Aws.Configuration;
using Liquid.Messaging.Aws.Extensions;
using Liquid.Messaging.Aws.Parameters;
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
    public class SnsProducer<TMessage> : ILightProducer<TMessage>
    {
        private readonly ILogger _logger;
        private readonly ILightContextFactory _contextFactory;
        private readonly AwsMessagingSettings _messagingSettings;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly SnsProducerParameter _snsProducerParameter;
        private AmazonSimpleNotificationServiceClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqsProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="snsProducerParameter">The SNS producer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException">messaging</exception>
        public SnsProducer(ILightContextFactory contextFactory,
                           ILightTelemetryFactory telemetryFactory,
                           ILoggerFactory loggerFactory,
                           ILightMessagingConfiguration<AwsMessagingSettings> messagingConfiguration,
                           SnsProducerParameter snsProducerParameter)
        {
            _snsProducerParameter = snsProducerParameter;
            _contextFactory = contextFactory;
            _telemetryFactory = telemetryFactory;
            _messagingSettings = messagingConfiguration?.GetSettings(_snsProducerParameter.ConnectionId) ?? 
                    throw new MessagingMissingConfigurationException(_snsProducerParameter.ConnectionId);
            _logger = loggerFactory.CreateLogger(typeof(SqsProducer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the client.
        /// </summary>
        private void InitializeClient()
        {
            var credentials = new BasicAWSCredentials(_messagingSettings.AccessKey, _messagingSettings.SecretKey);
            var config = new AmazonSimpleNotificationServiceConfig()
            {
                ServiceURL = _messagingSettings.ConnectionString,
                RegionEndpoint = _messagingSettings.GetRegion()
            };

            _client = new AmazonSimpleNotificationServiceClient(credentials, config);
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
                var telemetryKey = $"SnsProducer_{_snsProducerParameter.Topic}";

                telemetry.AddContext($"SendMessage_{nameof(TMessage)}");
                telemetry.StartTelemetryStopWatchMetric(telemetryKey);
                var aggregationId = context.GetAggregationId();

                if (!string.IsNullOrEmpty(context.ContextCulture)) customHeaders.TryAdd("liquidCulture", context.ContextCulture);
                if (!string.IsNullOrEmpty(context.ContextChannel)) customHeaders.TryAdd("liquidChannel", context.ContextChannel);
                customHeaders.TryAdd("liquidCorrelationId", context.ContextId.ToString());
                customHeaders.TryAdd("liquidBusinessCorrelationId", context.BusinessContextId.ToString());
                customHeaders.TryAdd("liquidAggregationId", aggregationId);

                var messageBody = message.ToString();
                if (_snsProducerParameter.MessageStructure.Equals("json", StringComparison.InvariantCultureIgnoreCase))
                {
                    messageBody = !_snsProducerParameter.CompressMessage ? message.ToJson() : Convert.ToBase64String(message.ToJson().GzipCompress());
                }

                if (_snsProducerParameter.CompressMessage) { customHeaders.TryAdd(CommonExtensions.ContentTypeHeader, CommonExtensions.GZipContentType); }

                var topicResponse = await _client.CreateTopicAsync(new CreateTopicRequest(_snsProducerParameter.Topic));

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
                    compressed = _snsProducerParameter.CompressMessage
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