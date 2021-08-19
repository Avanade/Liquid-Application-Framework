using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using AutoMapper;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Messaging.Aws.Configuration;
using Liquid.Messaging.Aws.Extensions;
using Liquid.Messaging.Aws.Parameters;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Aws
{
    /// <summary>
    /// AWS SQS Consumer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    /// <seealso cref="ILightConsumer{TMessage}" />
    public abstract class SqsConsumer<TMessage> : ILightConsumer<TMessage>, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AwsMessagingSettings _messagingSettings;
        private readonly ILiquidContext _contextFactory;
        private readonly SqsConsumerParameter _sqsConsumerParameter;
        private readonly CancellationTokenSource _cancellationToken;
        private IAmazonSQS _client;
        private string _queueUrl;

        private Func<TMessage, IDictionary<string, object>, CancellationToken, Task<bool>> _messageHandler;

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
        /// Initializes a new instance of the <see cref="SqsConsumer{TMessage}" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="sqsConsumerParameter">The SQS consumer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException">messaging</exception>
        protected SqsConsumer(IServiceProvider serviceProvider,
                              IMediator mediator,
                              IMapper mapper,
                              ILiquidContext contextFactory,
                              ILoggerFactory loggerFactory,
                              ILiquidConfiguration<AwsMessagingSettings> messagingConfiguration,
                              SqsConsumerParameter sqsConsumerParameter)
        {
            _sqsConsumerParameter = sqsConsumerParameter;
            _serviceProvider = serviceProvider;
            _contextFactory = contextFactory;
            _messagingSettings = messagingConfiguration?.Settings ??
                    throw new MessagingMissingConfigurationException(_sqsConsumerParameter.ConnectionId);

            MediatorService = mediator;
            MapperService = mapper;

            LogService = loggerFactory.CreateLogger(typeof(SqsConsumer<TMessage>).FullName);
            _cancellationToken = new CancellationTokenSource();
            InitializeClient();
        }

        private void InitializeClient()
        {
            _messageHandler = ConsumeAsync;
            var awsCredentials = new BasicAWSCredentials(_messagingSettings.AccessKey, _messagingSettings.SecretKey);
            var awsSqsConfig = new AmazonSQSConfig
            {
                ServiceURL = _messagingSettings.ConnectionString,
                RegionEndpoint = _messagingSettings.GetRegion()
            };

            _client = new AmazonSQSClient(awsCredentials, awsSqsConfig);

            Task.Run(StartProcessMessages);
        }

        private async Task StartProcessMessages()
        {
            try
            {
                _queueUrl = await _client.GetAwsQueueUrlAsync(_sqsConsumerParameter.Queue);
                var receiveMessageRequest = new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    AttributeNames = new List<string> { "All" },
                    MessageAttributeNames = new List<string> { "All" }
                };

                //Polling messages from SQS
                while (!_cancellationToken.Token.IsCancellationRequested)
                {
                    var receiveMessageResponse = await _client.ReceiveMessageAsync(receiveMessageRequest);
                    receiveMessageResponse?.Messages?.ForEach(async message => await ProcessMessageAsync(message));
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex, ex.Message);
                throw new MessagingConsumerException(ex);
            }
        }

        private async Task ProcessMessageAsync(Message message)
        {

            try
            {

                var eventMessage = message.MessageAttributes.ContainsKey(CommonExtensions.ContentTypeHeader) &&
                                   message.MessageAttributes[CommonExtensions.ContentTypeHeader].StringValue.Equals(CommonExtensions.GZipContentType, StringComparison.InvariantCultureIgnoreCase)
                    ? Convert.FromBase64String(message.Body).GzipDecompress().ParseJson<TMessage>()
                    : message.Body.ParseJson<TMessage>();

                using (_serviceProvider.CreateScope())
                {
                    var context = _contextFactory;

                    context.Upsert("MessageId", message.MessageId);

                    var headers = message.MessageAttributes.GetCustomHeaders();

                    if (message.MessageAttributes != null) { AddContextHeaders(message.MessageAttributes, context); }

                    var messageProcessed = await _messageHandler.Invoke(eventMessage, headers, _cancellationToken.Token);
                    if (messageProcessed || _sqsConsumerParameter.AutoComplete)
                    {
                        var deleteMessageRequest = new DeleteMessageRequest(_queueUrl, message.ReceiptHandle);
                        await _client.DeleteMessageAsync(deleteMessageRequest);
                    }

                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Adds the context headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="context">The context.</param>
        private static void AddContextHeaders(IDictionary<string, MessageAttributeValue> headers, ILiquidContext context)
        {
            if (headers.TryGetValue("Culture", out var culture) && culture?.ToString().IsNotNullOrEmpty() == true)
                context.Upsert("culture", culture.ToString());
            if (headers.TryGetValue("Channel", out var channel) && channel?.ToString().IsNotNullOrEmpty() == true)
                context.Upsert("Channel", channel.ToString());
            if (headers.TryGetValue("CorrelationId", out var contextId) && contextId?.ToString().IsNotNullOrEmpty() == true)
                context.Upsert("CorrelationId", contextId.ToString().ToGuid());
            if (headers.TryGetValue("BusinessCorrelationId", out var businessContextId) && businessContextId?.ToString().IsNotNullOrEmpty() == true)
                context.Upsert("BusinessCorrelationId", businessContextId.ToString().ToGuid());
            if (headers.TryGetValue("AggregationId", out var aggregationId) && aggregationId?.ToString().IsNotNullOrEmpty() == true)
                context.Upsert("AggregationId", aggregationId.ToString());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="dispose"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool dispose)
        {
            _cancellationToken?.Cancel();
            _cancellationToken?.Dispose();
            _client?.Dispose();
        }
    }
}