using AutoMapper;
using Confluent.Kafka;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.Kafka.Configuration;
using Liquid.Messaging.Kafka.Extensions;
using Liquid.Messaging.Kafka.Parameters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.Kafka
{
    /// <summary>
    /// Azure Service Bus Consumer Class.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message object.</typeparam>
    /// <seealso cref="ILightConsumer{TMessage}" />
    /// <seealso cref="IDisposable" />
    public abstract class KafkaConsumer<TMessage> : ILightConsumer<TMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly KafkaSettings _messagingSettings;
        private readonly ILiquidContext _contextFactory;
        private readonly KafkaConsumerParameter _kafkaConsumerParameter;
        private readonly CancellationTokenSource _cancellationToken;
        private readonly Func<TMessage, IDictionary<string, object>, CancellationToken, Task<bool>> _messageHandler;
        private IConsumer<Ignore, string> _client;

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
        /// Initializes a new instance of the <see cref="KafkaConsumer{TMessage}" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="mediator">The mediator.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="messagingConfiguration">The messaging configuration.</param>
        /// <param name="kafkaConsumerParameter">The kafka consumer parameter.</param>
        /// <exception cref="MessagingMissingConfigurationException"></exception>
        protected KafkaConsumer(IServiceProvider serviceProvider,
                                IMediator mediator,
                                IMapper mapper,
                                ILiquidContext contextFactory,
                                ILoggerFactory loggerFactory,
                                ILiquidConfiguration<KafkaSettings> messagingConfiguration,
                                KafkaConsumerParameter kafkaConsumerParameter)
        {
            _kafkaConsumerParameter = kafkaConsumerParameter;
            _serviceProvider = serviceProvider;
            _contextFactory = contextFactory;
            _messagingSettings = messagingConfiguration?.Settings ??
                    throw new MessagingMissingConfigurationException(_kafkaConsumerParameter.ConnectionId);
            _cancellationToken = new CancellationTokenSource();
            MediatorService = mediator;
            MapperService = mapper;
            LogService = loggerFactory.CreateLogger(typeof(KafkaConsumer<TMessage>).FullName);
            _messageHandler = ConsumeAsync;
            Task.Run(StartProcessMessages);
        }

        /// <summary>
        /// Starts the process messages.
        /// </summary>
        /// <exception cref="MessagingConsumerException"></exception>
        private async Task StartProcessMessages()
        {
            try
            {
                //TODO: Review connection parameters with kafka.
                var config = new ConsumerConfig
                {
                    SocketKeepaliveEnable = _messagingSettings.SocketKeepAlive,
                    SocketTimeoutMs = _messagingSettings.Timeout,
                    BootstrapServers = _messagingSettings.ConnectionString,
                    ClientId = _kafkaConsumerParameter.ConnectionId,
                    EnableAutoCommit = false //this is done for fine tuning below
                };

                _client = new ConsumerBuilder<Ignore, string>(config).Build();
                _client.Subscribe(_kafkaConsumerParameter.Topic);

                //Polling messages from Kafka
                while (!_cancellationToken.Token.IsCancellationRequested)
                {
                    var response = _client.Consume(_cancellationToken.Token);
                    await ProcessMessageAsync(response);
                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex, ex.Message);
                throw new MessagingConsumerException(ex);
            }
        }

        /// <summary>
        /// Processes the message asynchronous.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <exception cref="MessagingConsumerException"></exception>
        private async Task ProcessMessageAsync(ConsumeResult<Ignore, string> response)
        {           
            try
            {

                var headers = response.Message.Headers.GetCustomHeaders();

                var eventMessage = headers.ContainsKey(CommonExtensions.ContentTypeHeader) &&
                                   headers[CommonExtensions.ContentTypeHeader].ToString().Equals(CommonExtensions.GZipContentType, StringComparison.InvariantCultureIgnoreCase)
                    ? Encoding.UTF8.GetBytes(response.Message.Value).GzipDecompress().ParseJson<TMessage>()
                    : response.Message.Value.ParseJson<TMessage>();

                using (_serviceProvider.CreateScope())
                {
                    var context = _contextFactory;

                    AddContextHeaders(headers, context);

                    var messageProcessed = await _messageHandler.Invoke(eventMessage, headers, _cancellationToken.Token);
                    if (messageProcessed || _kafkaConsumerParameter.AutoComplete)
                    {
                        try
                        {
                            _client.Commit(response);
                        }
                        catch (KafkaException e)
                        {
                            Console.WriteLine($"Commit error: {e.Error.Reason}");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogService.LogError(ex, ex.Message);
                throw new MessagingConsumerException(ex);
            }
        }

        /// <summary>
        /// Adds the context headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="context">The context.</param>
        private static void AddContextHeaders(IDictionary<string, object> headers, ILiquidContext context)
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
            if (headers.TryGetValue("MessageId", out var messageId) && messageId?.ToString().IsNotNullOrEmpty() == true)
                context.Upsert("MessageId", messageId.ToString());
        }
    }
}