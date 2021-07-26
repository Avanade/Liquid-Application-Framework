using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Core;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.Gcp.Configuration;
using Liquid.Messaging.Gcp.Extensions;
using Liquid.Messaging.Gcp.Factories;
using Liquid.Messaging.Gcp.Parameters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liquid.Messaging.Gcp
{
    /// <summary>
    /// Google Cloud Pub/Sub Producer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the object.</typeparam>
    /// <seealso cref="ILightProducer{TMessage}" />
    /// <seealso cref="IDisposable" />
    public class PubSubProducer<TMessage> : ILightProducer<TMessage>
    {
        private readonly ILogger _logger;
        private readonly ILiquidContext _context;
        private readonly PubSubSettings _messagingSettings;
        private readonly PubSubProducerParameter _pubSubProducerParameter;
        private readonly IPubSubClientFactory _pubSubClientFactory;
        private PublisherServiceApiClient _client;
        private TopicName _topicName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PubSubProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="pubSubClientFactory">The pub sub client factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="pubSubProducerParameter">The pub sub producer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException">messaging</exception>
        public PubSubProducer(ILiquidContext contextFactory,
                                 ILoggerFactory loggerFactory,
                                 IPubSubClientFactory pubSubClientFactory,
                                 ILiquidConfiguration<PubSubSettings> messagingConfiguration,
                                 PubSubProducerParameter pubSubProducerParameter)
        {
            _pubSubProducerParameter = pubSubProducerParameter;
            _context = contextFactory;
            _messagingSettings = messagingConfiguration?.Settings ??
                    throw new MessagingMissingConfigurationException(_pubSubProducerParameter.ConnectionId);
            _logger = loggerFactory.CreateLogger(typeof(PubSubProducer<TMessage>).FullName);
            _pubSubClientFactory = pubSubClientFactory;
            InitializeClient();
        }

        /// <summary>
        /// Initializes the Google Cloud Pub/Sub Client.
        /// </summary>
        private void InitializeClient()
        {
            _client = _pubSubClientFactory.GetPublisher(_messagingSettings);

            _topicName = new TopicName(_messagingSettings.ProjectId, _pubSubProducerParameter.Topic);
            try
            {
                _client.CreateTopic(_topicName);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                _logger.LogWarning($"Topic {_pubSubProducerParameter.Topic} already exists.");
            }
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

                foreach (var item in context.current)
                {
                    customHeaders.TryAdd(item.Key, item.Value);
                }

                if (_pubSubProducerParameter.CompressMessage) { customHeaders.TryAdd(CommonExtensions.ContentTypeHeader, CommonExtensions.GZipContentType); }

                var messageBytes = !_pubSubProducerParameter.CompressMessage ? message.ToJsonBytes() : message.ToJson().GzipCompress();
                var request = new PubsubMessage { Data = ByteString.CopyFrom(messageBytes) };
                request.Attributes.AddCustomHeaders(customHeaders);

                var messageId = await _client.PublishAsync(_topicName, new[] { request });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new MessagingProducerException(ex);
            }
        }
    }
}