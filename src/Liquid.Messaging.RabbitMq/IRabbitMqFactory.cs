using Liquid.Messaging.RabbitMq.Settings;
using RabbitMQ.Client;

namespace Liquid.Messaging.RabbitMq
{
    /// <summary>
    /// RabbitMq <see cref="IModel"/> provider.
    /// </summary>
    public interface IRabbitMqFactory
    {
        /// <summary>
        /// Initialize and return a new instance of <see cref="IModel"/>
        /// </summary>
        /// <param name="settings">RabbitMq producer configuration properties set.</param>
        IModel GetSender(RabbitMqProducerSettings settings);

        /// <summary>
        /// Initialize and return a new instance of <see cref="IModel"/>
        /// </summary>
        /// <param name="settings">RabbitMq consumer configuration properties set.</param>
        IModel GetReceiver(RabbitMqConsumerSettings settings);
    }
}
