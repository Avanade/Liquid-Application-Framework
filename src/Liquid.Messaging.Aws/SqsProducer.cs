using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
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

namespace Liquid.Messaging.Aws
{
    /// <summary>
    /// AWS SQS Queue Producer class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message object.</typeparam>
    public class SqsProducer<TMessage> : ILightProducer<TMessage>
    {
        private readonly ILogger _logger;
        private readonly ILightContextFactory _contextFactory;
        private readonly AwsMessagingSettings _messagingSettings;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly SqsProducerParameter _sqsProducerParameter;
        private IAmazonSQS _client;
        private string _queueUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqsProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="sqsProducerParameter">The SQS producer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException">messaging</exception>
        public SqsProducer(ILightContextFactory contextFactory,
                              ILightTelemetryFactory telemetryFactory,
                              ILoggerFactory loggerFactory,
                              ILightMessagingConfiguration<AwsMessagingSettings> messagingConfiguration,
                              SqsProducerParameter sqsProducerParameter)
        {

            _sqsProducerParameter = sqsProducerParameter;
            _contextFactory = contextFactory;
            _telemetryFactory = telemetryFactory;
            _messagingSettings = messagingConfiguration?.GetSettings(_sqsProducerParameter.ConnectionId) ?? 
                    throw new MessagingMissingConfigurationException(_sqsProducerParameter.ConnectionId);
            _logger = loggerFactory.CreateLogger(typeof(SqsProducer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the client.
        /// </summary>
        private void InitializeClient()
        {
            var awsCredentials = new BasicAWSCredentials(_messagingSettings.AccessKey, _messagingSettings.SecretKey);
            var awsSqsConfig = new AmazonSQSConfig
            {
                ServiceURL = _messagingSettings.ConnectionString,
                RegionEndpoint = _messagingSettings.GetRegion()
            };

            _client = new AmazonSQSClient(awsCredentials, awsSqsConfig);

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
                var telemetryKey = $"SqsProducer_{_sqsProducerParameter.Queue}";

                telemetry.AddContext($"SendMessage_{nameof(TMessage)}");
                telemetry.StartTelemetryStopWatchMetric(telemetryKey);
                var aggregationId = context.GetAggregationId();
                _queueUrl = await _client.GetAwsQueueUrlAsync(_sqsProducerParameter.Queue);

                if (!string.IsNullOrEmpty(context.ContextCulture)) customHeaders.TryAdd("liquidCulture", context.ContextCulture);
                if (!string.IsNullOrEmpty(context.ContextChannel)) customHeaders.TryAdd("liquidChannel", context.ContextChannel);
                customHeaders.TryAdd("liquidCorrelationId", context.ContextId.ToString());
                customHeaders.TryAdd("liquidBusinessCorrelationId", context.BusinessContextId.ToString());
                customHeaders.TryAdd("liquidAggregationId", aggregationId);
                if (_sqsProducerParameter.CompressMessage) { customHeaders.TryAdd(CommonExtensions.ContentTypeHeader, CommonExtensions.GZipContentType); }
                
                var messageBody = !_sqsProducerParameter.CompressMessage ? message.ToJson() : Convert.ToBase64String(message.ToJson().GzipCompress());

                var request = new SendMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MessageBody = messageBody,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>().AddCustomHeaders(customHeaders)
                };

                await _client.SendMessageAsync(request);

                telemetry.CollectTelemetryStopWatchMetric(telemetryKey, new
                {
                    producer = telemetryKey,
                    messageType = typeof(TMessage).FullName,
                    aggregationId,
                    message,
                    headers = customHeaders,
                    compressed = _sqsProducerParameter.CompressMessage
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