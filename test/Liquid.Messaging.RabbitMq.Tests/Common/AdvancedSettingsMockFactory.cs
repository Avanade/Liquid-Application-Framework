using Liquid.Messaging.RabbitMq.Configuration;

namespace Liquid.Messaging.RabbitMq.Tests.Common
{
    internal class AdvancedSettingsMockFactory
    {
        public static RabbitMqParameterSettings GetAdvancedSettings()
        {
            return new RabbitMqParameterSettings
            {
                ExchangeType = "topic",
                AutoAck = false,
                Expiration = "60000",
                Persistent = false,
                AutoDelete = false,
                Durable = false,
                QueueArguments = null,
                ExchangeArguments = null
            };
        }
    }
}
