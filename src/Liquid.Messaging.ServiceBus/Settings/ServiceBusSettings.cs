using Liquid.Core.Attributes;

namespace Liquid.Messaging.ServiceBus.Settings
{
    /// <summary>
    /// Service bus configuration properties set.
    /// </summary>
    [LiquidSectionName("Liquid:Messaging:ServiceBus")]
    public class ServiceBusSettings
    {
        /// <summary>
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
