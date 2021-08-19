using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
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

namespace Liquid.Messaging.Aws
{
    /// <summary>
    /// AWS SQS Queue Producer class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message object.</typeparam>
    public class SqsProducer<TMessage> : ILightProducer<TMessage>
    {
        private readonly ILogger _logger;
        private readonly ILiquidContext _context;
        private readonly AwsMessagingSettings _messagingSettings;
        private readonly SqsProducerParameter _sqsProducerParameter;
        private IAmazonSQS _client;
        private string _queueUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqsProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="context">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="sqsProducerParameter">The SQS producer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException">messaging</exception>
        public SqsProducer(ILiquidContext context,
                              ILoggerFactory loggerFactory,
                              ILiquidConfiguration<AwsMessagingSettings> messagingConfiguration,
                              SqsProducerParameter sqsProducerParameter)
        {

            _sqsProducerParameter = sqsProducerParameter;
            _context = context;
            _messagingSettings = messagingConfiguration?.Settings ??
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
            if (customHeaders == null) customHeaders = new Dictionary<string, object>();

            try
            {
                var context = _context;

                _queueUrl = await _client.GetAwsQueueUrlAsync(_sqsProducerParameter.Queue);

                foreach (var item in context.current)
                {
                    customHeaders.TryAdd(item.Key, item.Value);
                }

                if (_sqsProducerParameter.CompressMessage) { customHeaders.TryAdd(CommonExtensions.ContentTypeHeader, CommonExtensions.GZipContentType); }

                var messageBody = !_sqsProducerParameter.CompressMessage ? message.ToJson() : Convert.ToBase64String(message.ToJson().GzipCompress());

                var request = new SendMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MessageBody = messageBody,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>().AddCustomHeaders(customHeaders)
                };

                await _client.SendMessageAsync(request);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new MessagingProducerException(ex);
            }
        }
    }
}