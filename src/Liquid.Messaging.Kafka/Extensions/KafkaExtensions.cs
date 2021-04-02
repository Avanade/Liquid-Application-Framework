using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Liquid.Core.Utils;

namespace Liquid.Messaging.Kafka.Extensions
{
    /// <summary>
    /// Kafka Extensions Class.
    /// </summary>
    internal static class KafkaExtensions
    {
        /// <summary>
        /// Adds the custom headers.
        /// </summary>
        /// <param name="messageAttributes">The message attributes.</param>
        /// <param name="customHeaders">The custom headers.</param>
        public static Headers AddCustomHeaders(this Headers messageAttributes, IDictionary<string, object> customHeaders)
        {
            foreach (var (key, value) in customHeaders)
            {
                if (value != null)
                {
                    messageAttributes.Add(key, Encoding.UTF8.GetBytes(value.ToString()));
                }
            }

            return messageAttributes;
        }

        /// <summary>
        /// Gets the custom headers.
        /// </summary>
        /// <param name="messageAttributes">The message attributes.</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetCustomHeaders(this Headers messageAttributes)
        {
            var returnValue = new Dictionary<string, object>();

            foreach (var header in messageAttributes)
            {
                returnValue.TryAdd(header.Key, Encoding.UTF8.GetString(header.GetValueBytes()));
            }

            return returnValue;
        }
    }
}