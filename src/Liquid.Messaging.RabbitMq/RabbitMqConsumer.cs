using Liquid.Core.Extensions;
using Liquid.Core.Utils;
using Liquid.Messaging.Exceptions;
using Liquid.Messaging.Extensions;
using Liquid.Messaging.Interfaces;
using Liquid.Messaging.RabbitMq.Settings;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Messaging.RabbitMq
{
    /// <summary>
    /// RabbitMq Consumer Adapter Class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the message object.</typeparam>
    /// <seealso cref="ILiquidConsumer{TEntity}" />
    /// <seealso cref="IDisposable" />
    public class RabbitMqConsumer<TEntity> : ILiquidConsumer<TEntity>
    {

        private readonly bool _autoAck;
        private IModel _channelModel;
        private readonly IRabbitMqFactory _factory;
        private readonly RabbitMqConsumerSettings _settings;

        ///<inheritdoc/>
        public event Func<ProcessMessageEventArgs<TEntity>, CancellationToken, Task> ProcessMessageAsync;

        ///<inheritdoc/>
        public event Func<ProcessErrorEventArgs, Task> ProcessErrorAsync;

        /// <summary>
        /// Initilize an instance of <see cref="RabbitMqConsumer{TEntity}"/>
        /// </summary>
        /// <param name="factory">RabbitMq client factory.</param>
        /// <param name="settings">Configuration properties set.</param>
        public RabbitMqConsumer(IRabbitMqFactory factory, RabbitMqConsumerSettings settings)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            _autoAck = _settings.AdvancedSettings?.AutoAck ?? true;
        }

        ///<inheritdoc/>
        public void RegisterMessageHandler()
        {
            if (ProcessMessageAsync is null)
            {
                throw new NotImplementedException($"The {nameof(ProcessMessageAsync)} action must be added to class.");
            }

            _channelModel = _factory.GetReceiver(_settings);

            var consumer = new EventingBasicConsumer(_channelModel);

            consumer.Received += async (model, deliverEvent) => { await MessageHandler(deliverEvent, new CancellationToken()); };

            _channelModel.BasicConsume(_settings.Queue, _autoAck, consumer);
        }

        /// <summary>
        /// Process incoming messages.
        /// </summary>
        /// <param name="deliverEvent">Message to be processed.</param>
        /// <param name="cancellationToken"> Propagates notification that operations should be canceled.</param>
        protected async Task MessageHandler(BasicDeliverEventArgs deliverEvent, CancellationToken cancellationToken)
        {
            try
            {
                await ProcessMessageAsync(GetEventArgs(deliverEvent), cancellationToken);

                if (!_autoAck)
                {
                    _channelModel.BasicAck(deliverEvent.DeliveryTag, false);
                }
            }
            catch (Exception)
            {
                if (!_autoAck)
                {
                    _channelModel.BasicNack(deliverEvent.DeliveryTag, false, true);
                }
            }
        }

        private ProcessMessageEventArgs<TEntity> GetEventArgs(BasicDeliverEventArgs deliverEvent)
        {
            var data = deliverEvent.BasicProperties?.ContentType == CommonExtensions.GZipContentType
                   ? deliverEvent.Body.ToArray().GzipDecompress().ParseJson<TEntity>()
                   : deliverEvent.Body.ToArray().ParseJson<TEntity>();

            var headers = deliverEvent.BasicProperties?.Headers;

            return new ProcessMessageEventArgs<TEntity> { Data = data, Headers = headers };
        }
    }
}