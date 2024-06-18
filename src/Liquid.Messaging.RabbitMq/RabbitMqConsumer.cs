using Liquid.Core.Entities;
using Liquid.Core.Extensions;
using Liquid.Core.Interfaces;
using Liquid.Core.Utils;
using Liquid.Messaging.RabbitMq.Settings;
using Microsoft.Extensions.Options;
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
        public event Func<ConsumerMessageEventArgs<TEntity>, CancellationToken, Task> ConsumeMessageAsync;

        ///<inheritdoc/>
        public event Func<ConsumerErrorEventArgs, Task> ProcessErrorAsync;

        /// <summary>
        /// Initilize an instance of <see cref="RabbitMqConsumer{TEntity}"/>
        /// </summary>
        /// <param name="factory">RabbitMq client factory.</param>
        /// <param name="settings">Configuration properties set.</param>
        public RabbitMqConsumer(IRabbitMqFactory factory, IOptions<RabbitMqConsumerSettings> settings)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

            _autoAck = _settings.AdvancedSettings?.AutoAck ?? true;
        }

        ///<inheritdoc/>
        public void RegisterMessageHandler()
        {
            if (ConsumeMessageAsync is null)
            {
                throw new NotImplementedException($"The {nameof(ConsumeMessageAsync)} action must be added to class.");
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
                await ConsumeMessageAsync(GetEventArgs(deliverEvent), cancellationToken);

                if (!_autoAck)
                {
                    _channelModel.BasicAck(deliverEvent.DeliveryTag, false);
                }
            }
            catch (Exception)
            {
                if (!_autoAck)
                {
                    var queueAckMode = _settings.AdvancedSettings.QueueAckModeSettings ?? new QueueAckModeSettings() { QueueAckMode = QueueAckModeEnum.BasicAck, Requeue = true };

                    switch (queueAckMode.QueueAckMode)
                    {
                        case QueueAckModeEnum.BasicAck:
                            _channelModel.BasicNack(deliverEvent.DeliveryTag, false, queueAckMode.Requeue);
                            break;
                        case QueueAckModeEnum.BasicReject:
                            _channelModel.BasicReject(deliverEvent.DeliveryTag, queueAckMode.Requeue);
                            break;
                        default:
                            _channelModel.BasicNack(deliverEvent.DeliveryTag, false, true);
                            break;
                    }

                }
            }
        }

        private ConsumerMessageEventArgs<TEntity> GetEventArgs(BasicDeliverEventArgs deliverEvent)
        {
            var data = deliverEvent.BasicProperties?.ContentType == CommonExtensions.GZipContentType
                   ? deliverEvent.Body.ToArray().GzipDecompress().ParseJson<TEntity>()
                   : deliverEvent.Body.ToArray().ParseJson<TEntity>();

            var headers = deliverEvent.BasicProperties?.Headers;

            return new ConsumerMessageEventArgs<TEntity> { Data = data, Headers = headers };
        }
    }
}