using Liquid.Messaging.Exceptions;
using Liquid.Messaging.RabbitMq.Settings;
using RabbitMQ.Client;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Messaging.RabbitMq
{
    ///<inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class RabbitMqFactory : IRabbitMqFactory
    {
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
                var connectionFactory = new ConnectionFactory
                {
                    Uri = new Uri(settings.QueueSettings.ConnectionString),
                    RequestedHeartbeat = TimeSpan.FromSeconds(settings.QueueSettings?.RequestHeartBeatInSeconds ?? 60),
                    AutomaticRecoveryEnabled = settings.QueueSettings?.AutoRecovery ?? true
                };

                var connection = connectionFactory.CreateConnection();
                var channelModel = connection.CreateModel();

                if (settings.QueueSettings.Prefetch.HasValue &&
                    settings.QueueSettings.PrefetchCount.HasValue &&
                    settings.QueueSettings.Global.HasValue)
                {
                    channelModel.BasicQos(settings.QueueSettings.Prefetch.Value,
                        settings.QueueSettings.PrefetchCount.Value,
                        settings.QueueSettings.Global.Value);
                }

                channelModel.QueueDeclare(settings.Queue,
                   settings.AdvancedSettings?.Durable ?? false,
                   settings.AdvancedSettings?.Exclusive ?? true,
                   settings.AdvancedSettings?.AutoDelete ?? false,
                   settings.AdvancedSettings?.QueueArguments);

                channelModel.QueueBind(settings.Queue, settings.Exchange, string.Empty);

                return channelModel;
            }
            catch(Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex,"for queue '" + settings?.Queue + "'");
            }

        }

        ///<inheritdoc/>
        public IModel GetSender(RabbitMqProducerSettings settings)
        {
            try
            {
                var connectionFactory = new ConnectionFactory
                {
                    Uri = new Uri(settings.QueueSettings.ConnectionString),
                    RequestedHeartbeat = TimeSpan.FromSeconds(settings.QueueSettings?.RequestHeartBeatInSeconds ?? 60),
                    AutomaticRecoveryEnabled = settings.QueueSettings?.AutoRecovery ?? true
                };

                var connection = connectionFactory.CreateConnection();
                var channelModel = connection.CreateModel();
                channelModel.ExchangeDeclare(settings?.Exchange,
                    settings.AdvancedSettings?.ExchangeType ?? "direct",
                    settings.AdvancedSettings?.Durable ?? false,
                    settings.AdvancedSettings?.AutoDelete ?? false,
                    settings.AdvancedSettings?.ExchangeArguments);

                return channelModel;
            }
            catch (Exception ex)
            {
                throw new MessagingMissingConfigurationException(ex, "for exange '" + settings?.Exchange + "'");
            }
        }
    }
}
