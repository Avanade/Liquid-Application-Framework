using Amazon.SimpleNotificationService.Model;
using System.Collections.Generic;

namespace Liquid.Messaging.Aws.Extensions
{
    internal static class SnsExtensions
    {
        /// <summary>
        /// Adds the custom headers.
        /// </summary>
        /// <param name="messageAttributes">The message attributes.</param>
        /// <param name="customHeaders">The custom headers.</param>
        public static Dictionary<string, MessageAttributeValue> AddCustomHeaders(this Dictionary<string, MessageAttributeValue> messageAttributes, IDictionary<string, object> customHeaders)
        {
            foreach (var (key, value) in customHeaders)
            {
                if (value != null)
                {
                    messageAttributes.TryAdd(key, new MessageAttributeValue { DataType = "String", StringValue = value.ToString() });
                }
            }

            return messageAttributes;
        }
    }
}