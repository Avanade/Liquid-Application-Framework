using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Google.Api.Gax.Grpc;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
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
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Liquid.Messaging.Gcp
{
    /// <summary>
    /// Google Cloud Pub/Sub Consumer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message object.</typeparam>
    /// <seealso cref="Liquid.Messaging.ILightConsumer{TMessage}" />
    /// <seealso cref="System.IDisposable" />
    public abstract class PubSubConsumer<TMessage> : ILightConsumer<TMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MessagingSettings _messagingSettings;
        private readonly ILightContextFactory _contextFactory;
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly PubSubConsumerAttribute _attribute;
        private SubscriberClient _client;

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        [ExcludeFromCodeCoverage]
        public ILogger LogService { get; }

        /// <summary>
        /// Gets the mapper service.
        /// </summary>
        /// <value>
        /// The mapper service.
        /// </value>
        [ExcludeFromCodeCoverage]
        public IMapper MapperService { get; }

        /// <summary>
        /// Gets the consumer mediator.
        /// </summary>
        /// <value>
        /// The mediator.
        /// </value>
        [ExcludeFromCodeCoverage]
        public IMediator MediatorService { get; }

        /// <summary>
        /// Consumes the message from  subscription asynchronous.
        /// </summary>
        /// <param name="message">The message to be consumed.</param>
        /// <param name="headers">The custom headers of message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public abstract Task<bool> ConsumeAsync(TMessage message, IDictionary<string, object> headers, CancellationToken cancellationToken);

        /// <summary>
        /// Initializes a new instance of the <see cref="PubSubConsumer{TMessage}" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingSettings">The messaging settings.</param>
        /// <exception cref="NotImplementedException">The {nameof(PubSubConsumerAttribute)} attribute decorator must be added to configuration class.</exception>
        protected PubSubConsumer(IServiceProvider serviceProvider,
                                 IMediator mediator,
                                 IMapper mapper,
                                 ILightContextFactory contextFactory,
                                 ILightTelemetryFactory telemetryFactory,
                                 ILoggerFactory loggerFactory,
                                 ILightConfiguration<List<MessagingSettings>> messagingSettings)
        {
            if (!GetType().GetCustomAttributes(typeof(PubSubConsumerAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(PubSubConsumerAttribute)} attribute decorator must be added to configuration class.");
            }
            _attribute = GetType().GetCustomAttribute<PubSubConsumerAttribute>(true);
            _telemetryFactory = telemetryFactory;
            _serviceProvider = serviceProvider;
            _contextFactory = contextFactory;
            _messagingSettings = messagingSettings?.Settings?.GetMessagingSettings(_attribute.ConnectionId) ?? throw new MessagingMissingConfigurationException("messaging");

            MediatorService = mediator;
            MapperService = mapper;
            LogService = loggerFactory.CreateLogger(typeof(PubSubConsumer<TMessage>).FullName);
            InitializeClient();
        }

        /// <summary>
        /// Initializes the client.
        /// </summary>
        private void InitializeClient()
        {
            var topicName = new TopicName(_messagingSettings.GetProjectId(), _attribute.Topic);
            var subscriptionName = new SubscriptionName(_messagingSettings.GetProjectId(), _attribute.Subscription);
            var credentialPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _messagingSettings.ConnectionString);
            var credentials = CallCredentials.FromInterceptor(GoogleAuthInterceptors.FromCredential(GoogleCredential.FromFile(credentialPath)));
            var serviceClientApiBuilder =
                new SubscriberServiceApiClientBuilder
                {
                    Settings = new SubscriberServiceApiSettings
                    {
#pragma warning disable CS0618
                        CallSettings = CallSettings.FromCallCredentials(credentials)
#pragma warning restore CS0618
                    }
                };
            var subscriberService = serviceClientApiBuilder.Build();

            try
            {
                subscriberService.CreateSubscription(subscriptionName, topicName, null, 60);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                LogService.LogWarning($"Subscription {_attribute.Topic}, from topic {_attribute.Topic} already exists.");
            }


            Task.Run(async () =>
            {
                _client = await SubscriberClient.CreateAsync(subscriptionName,
                new SubscriberClient.ClientCreationSettings(subscriberServiceApiSettings:
                new SubscriberServiceApiSettings
                {
#pragma warning disable CS0618
                    CallSettings = CallSettings.FromCallCredentials(credentials)
#pragma warning restore CS0618
                }));
                await ProcessMessagesAsync(ConsumeAsync);
            });

        }

        /// <summary>
        /// Processes the messages asynchronous.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public async Task ProcessMessagesAsync(Func<TMessage, IDictionary<string, object>, CancellationToken, Task<bool>> handler)
        {
            await _client.StartAsync(async (message, cancellationToken) =>
            {
                var telemetry = _telemetryFactory.GetTelemetry();
                try
                {
                    var telemetryKey = $"PubSubConsumer_{_attribute.Topic}_{_attribute.Subscription}";

                    telemetry.AddContext($"ConsumeMessage_{nameof(TMessage)}");
                    telemetry.StartTelemetryStopWatchMetric(telemetryKey);

                    var eventMessage = message.Attributes.ContainsKey(CommonExtensions.ContentTypeHeader) &&
                                       message.Attributes[CommonExtensions.ContentTypeHeader].Equals(CommonExtensions.GZipContentType, StringComparison.InvariantCultureIgnoreCase)
                        ? message.Data.ToByteArray().GzipDecompress().ParseJson<TMessage>()
                        : message.Data.ToByteArray().ParseJson<TMessage>();

                    using (_serviceProvider.CreateScope())
                    {
                        var context = _contextFactory.GetContext();

                        var headers = message.Attributes.GetCustomHeaders();

                        if (headers != null) { AddContextHeaders(headers, context); }

                        var messageProcessed = await handler.Invoke(eventMessage, headers, cancellationToken);

                        telemetry.CollectTelemetryStopWatchMetric(telemetryKey, new
                        {
                            consumer = telemetryKey,
                            messageType = typeof(TMessage).FullName,
                            aggregationId = context.GetAggregationId(),
                            messageId = message.MessageId,
                            message = eventMessage,
                            processed = _attribute.AutoComplete || messageProcessed,
                            autoComplete = _attribute.AutoComplete,
                            headers
                        });

                        return messageProcessed || _attribute.AutoComplete ?
                            await Task.FromResult(SubscriberClient.Reply.Ack) :
                            await Task.FromResult(SubscriberClient.Reply.Nack);
                    }
                }
                catch (Exception ex)
                {
                    LogService.LogError(ex, ex.Message);
                    throw new MessagingConsumerException(ex);
                }
                finally
                {
                    telemetry.RemoveContext($"ConsumeMessage_{nameof(TMessage)}");
                }
            });
        }

        /// <summary>
        /// Adds the context headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="context">The context.</param>
        private static void AddContextHeaders(IDictionary<string, object> headers, ILightContext context)
        {
            if (headers.TryGetValue("liquidCulture", out var culture) && culture?.ToString().IsNotNullOrEmpty() == true)
                context.SetCulture(culture.ToString());
            if (headers.TryGetValue("liquidChannel", out var channel) && channel?.ToString().IsNotNullOrEmpty() == true)
                context.SetChannel(channel.ToString());
            if (headers.TryGetValue("liquidCorrelationId", out var contextId) && contextId?.ToString().IsNotNullOrEmpty() == true)
                context.SetContextId(contextId.ToString().ToGuid());
            if (headers.TryGetValue("liquidBusinessCorrelationId", out var businessContextId) && businessContextId?.ToString().IsNotNullOrEmpty() == true)
                context.SetBusinessContextId(businessContextId.ToString().ToGuid());
            if (headers.TryGetValue("liquidAggregationId", out var aggregationId) && aggregationId?.ToString().IsNotNullOrEmpty() == true)
                context.SetAggregationId(aggregationId.ToString());
        }


    }
}