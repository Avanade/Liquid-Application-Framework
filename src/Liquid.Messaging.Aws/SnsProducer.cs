using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Messaging.Aws.Configuration;
using Liquid.Messaging.Aws.Extensions;
using Liquid.Messaging.Aws.Parameters;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly ILiquidContext _context;
        private readonly AwsMessagingSettings _messagingSettings;
        private readonly SnsProducerParameter _snsProducerParameter;
        private AmazonSimpleNotificationServiceClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqsProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="snsProducerParameter">The SNS producer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException">messaging</exception>
        public SnsProducer(ILiquidContext contextFactory,
                           ILoggerFactory loggerFactory,
                           ILiquidConfiguration<AwsMessagingSettings> messagingConfiguration,
                           SnsProducerParameter snsProducerParameter)
        {
            _snsProducerParameter = snsProducerParameter;
            _context = contextFactory;
            _messagingSettings = messagingConfiguration?.Settings ??
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

            if (customHeaders == null) customHeaders = new Dictionary<string, object>();

            try
            {
                var context = _context;
                var telemetryKey = $"SnsProducer_{_snsProducerParameter.Topic}";


                foreach (var item in context.current)
                {
                    customHeaders.TryAdd(item.Key, item.Value);
                }

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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new MessagingProducerException(ex);
            }
        }
    }
}