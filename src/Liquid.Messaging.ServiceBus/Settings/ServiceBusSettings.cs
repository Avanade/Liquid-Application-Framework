using Liquid.Core.Attributes;
using Liquid.Messaging.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.Messaging.ServiceBus.Settings
{
    /// <summary>
    /// Service bus configuration properties set.
    /// </summary>
    [LiquidSectionName("Liquid:Messaging:ServiceBus")]
    public class ServiceBusSettings
    {
        /// <summary>
        /// Service Bus configuration settings list.
        /// </summary>
        public IList<ServiceBusConfiguration> Settings { get; set; }

        /// <summary>
        /// Gets a set of Service Bus configuration properties <see cref="ServiceBusConfiguration"/>
        /// that contains property "SettingsName" with same value of <paramref name="settingsName"/>.
        /// </summary>
        /// <param name="settingsName">Identifier of configuration set.</param>
        public ServiceBusConfiguration GetSettings(string settingsName)
        {
            var config = Settings.FirstOrDefault(x => x.SettingsName == settingsName);

            if (config is null) throw new MessagingMissingSettingsException(settingsName);

            return config;
        }
    }

    /// <summary>
    /// Service bus configuration properties set.
    /// </summary>
    public class ServiceBusConfiguration
    {
        /// <summary>
        /// Identifier of configuration set.
        /// </summary>
        public string SettingsName { get; set; }

        // <summary>
        /// Connection string of Service Bus resource.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Topic or Queue path.
        /// </summary>
        public string EntityPath { get; set; }

        /// <summary>
        /// Indicates max number of concurrent consumer calls.
        /// The default value is 1.
        /// </summary>
        public int MaxConcurrentCalls { get; set; } = 1;

        /// <summary>
        ///Indicates whether the consumer must lock the message during execution and complete,
        ///abort or move to DLQ according to processing result, 
        ///otherwise it deletes message from queue/topic immediately after reading, 
        ///regardless of processing result.
        /// The default value is true.
        /// </summary>
        public bool PeekLockMode { get; set; } = true;
    }
}
