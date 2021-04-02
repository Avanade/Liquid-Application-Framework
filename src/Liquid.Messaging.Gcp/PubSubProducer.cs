using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Google.Api.Gax.Grpc;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Auth;
using Grpc.Core;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.Gcp.Attributes;
using Liquid.Messaging.Gcp.Extensions;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Gcp
{
    /// <summary>
    /// Google Cloud Pub/Sub Producer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the object.</typeparam>
    /// <seealso cref="Liquid.Messaging.ILightProducer{TMessage}" />
    /// <seealso cref="System.IDisposable" />
    public abstract class PubSubProducer<TMessage> : ILightProducer<TMessage>
    {
        private readonly ILogger _logger;
        private readonly ILightContextFactory _contextFactory;
        private readonly MessagingSettings _messagingSettings;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly PubSubProducerAttribute _attribute;
        private PublisherServiceApiClient _client;
        private TopicName _topicName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PubSubProducer{TMessage}" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        /// <exception cref="NotImplementedException">The {nameof(PubSubProducerAttribute)} attribute decorator must be added to configuration class.</exception>
        protected PubSubProducer(ILightContextFactory contextFactory,
                                 ILightTelemetryFactory telemetryFactory,
                                 ILoggerFactory loggerFactory,
                                 ILightConfiguration<List<MessagingSettings>> messagingSettings)
        {
            if (!GetType().GetCustomAttributes(typeof(PubSubProducerAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(PubSubProducerAttribute)} attribute decorator must be added to configuration class.");
            }
            _attribute = GetType().GetCustomAttribute<PubSubProducerAttribute>(true);
            _contextFactory = contextFactory;
            _telemetryFactory = telemetryFactory;
            _messagingSettings = messagingSettings?.Settings?.GetMessagingSettings(_attribute.ConnectionId) ?? throw new MessagingMissingConfigurationException("messaging");
            _logger = loggerFactory.CreateLogger(typeof(PubSubProducer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the Google Cloud Pub/Sub Client.
        /// </summary>
        private void InitializeClient()
        {
            var credentialPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _messagingSettings.ConnectionString);
            var credentials = CallCredentials.FromInterceptor(GoogleAuthInterceptors.FromCredential(GoogleCredential.FromFile(credentialPath)));
            var serviceClientApiBuilder = new PublisherServiceApiClientBuilder
            {
                Settings = new PublisherServiceApiSettings
                {
#pragma warning disable CS0618
                    CallSettings = CallSettings.FromCallCredentials(credentials)
#pragma warning restore CS0618
                }
            };

            _client = serviceClientApiBuilder.Build();

            _topicName = new TopicName(_messagingSettings.GetProjectId(), _attribute.Topic);
            try
            {
                _client.CreateTopic(_topicName);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                _logger.LogWarning($"Topic {_attribute.Topic} already exists.");
            }
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

                customHeaders.TryAdd("liquidCulture", context.ContextCulture);
                customHeaders.TryAdd("liquidChannel", context.ContextChannel);
                customHeaders.TryAdd("liquidCorrelationId", context.ContextId.ToString());
                customHeaders.TryAdd("liquidBusinessCorrelationId", context.BusinessContextId.ToString());
                customHeaders.TryAdd("liquidAggregationId", aggregationId);
                if (_attribute.CompressMessage) { customHeaders.TryAdd(CommonExtensions.ContentTypeHeader, CommonExtensions.GZipContentType); }

                var messageBytes = !_attribute.CompressMessage ? message.ToJsonBytes() : message.ToJson().GzipCompress();
                var request = new PubsubMessage { Data = ByteString.CopyFrom(messageBytes) };
                request.Attributes.AddCustomHeaders(customHeaders);

                var messageId = await _client.PublishAsync(_topicName, new[] { request });

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