using Liquid.Core.Attributes;

namespace Liquid.Messaging.Kafka.Settings
{
    /// <summary>
    /// Kafka configuration properties set.
    /// </summary>
    [LiquidSectionName("liquid:messaging:kafka:")]
    public class KafkaSettings
    {
        /// <summary>
        /// Bootstrap server connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Socket keep alive flag.
        /// </summary>
        public bool SocketKeepAlive { get; set; }
                
        /// <summary>
        /// Client identifier.
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// Topic to consumer subscribe to. A regex can be specified to subscribe to the set of
        /// all matching topics.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Default timeout for network requests.Producer: ProduceRequests will use the 
        /// lesser value of `socket.timeout.ms` and remaining `message.timeout.ms` for the 
        /// first message in the batch. Consumer: FetchRequests will use `fetch.wait.max.ms` 
        /// + `socket.timeout.ms`. Admin: Admin requests will use `socket.timeout.ms` or 
        /// explicitly set `rd_kafka_AdminOptions_set_operation_timeout()` value. default: 60000
        /// </summary>
        public int Timeout { get; set; } = 6000;

        /// <summary>
        /// Automatically and periodically commit offsets in the background. Note: setting
        /// this to false does not prevent the consumer from fetching previously committed
        /// start offsets. default: true 
        /// </summary>
        public bool EnableAutoCommit { get; set; } = true;

        /// <summary>
        /// Indicates whether to be a compressed message.
        /// </summary>
        public bool CompressMessage { get; set; }

        public int MyProperty { get; set; }

        /// <summary>
        /// Client group id string. All clients sharing the same group.id belong to the same group.
        /// </summary>
        public string GroupId { get; set; }
    }
}