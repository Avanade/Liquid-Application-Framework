using Liquid.Messaging.Exceptions;
using Liquid.Messaging.RabbitMq.Settings;
using RabbitMQ.Client;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Messaging.RabbitMq
{
    ///<inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class RabbitMqFactory : IRabbitMqFactory, IDisposable
    {

        private IConnection _connection;
        private IModel _model;
        private bool _disposed;

        /// <summary>
        /// Initialize a new instace of <see cref="RabbitMqFactory"/>
        /// </summary>
        public RabbitMqFactory()
        {
        }

        ///<inheritdoc/>
        public IModel GetReceiver(RabbitMqConsumerSettings settings)
        {
            try
            {
                if (_connection == null && _model == null)
                {
                    var connectionFactory = new ConnectionFactory
                    {
                        Uri = new Uri(settings.QueueSettings.ConnectionString),
                        RequestedHeartbeat = TimeSpan.FromSeconds(settings.QueueSettings?.RequestHeartBeatInSeconds ?? 60),
                        AutomaticRecoveryEnabled = settings.QueueSettings?.AutoRecovery ?? true
                    };

                    _connection = connectionFactory.CreateConnection(settings.QueueSettings.ConnectionName);
                    _model = _connection.CreateModel();
                }

                if (settings.QueueSettings.Prefetch.HasValue &&
                    settings.QueueSettings.PrefetchCount.HasValue &&
                    settings.QueueSettings.Global.HasValue)
                {
                    _model.BasicQos(settings.QueueSettings.Prefetch.Value,
                        settings.QueueSettings.PrefetchCount.Value,
                        settings.QueueSettings.Global.Value);
                }

                _model.QueueBind(settings.Queue, settings.Exchange, string.Empty);

                return _model;
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex, "for queue '" + settings?.Queue + "'");
            }

        }

        ///<inheritdoc/>
        public IModel GetSender(RabbitMqProducerSettings settings)
        {
            try
            {
                if (_connection == null && _model == null)
                {
                    var connectionFactory = new ConnectionFactory
                    {
                        Uri = new Uri(settings.QueueSettings.ConnectionString),
                        RequestedHeartbeat = TimeSpan.FromSeconds(settings.QueueSettings?.RequestHeartBeatInSeconds ?? 60),
                        AutomaticRecoveryEnabled = settings.QueueSettings?.AutoRecovery ?? true
                    };

                    _connection = connectionFactory.CreateConnection(settings.QueueSettings.ConnectionName);
                    _model = _connection.CreateModel();
                }

                return _model;
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex, "for exange '" + settings?.Exchange + "'");
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Indicates if method should perform dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _connection?.Dispose();
                    _model?.Dispose();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
