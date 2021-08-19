using Google.Protobuf.Collections;
using System.Collections.Generic;

namespace Liquid.Messaging.Gcp.Extensions
{
    internal static class PubSubExtensions
    {
        /// <summary>
        /// Adds the custom headers.
        /// </summary>
        /// <param name="messageAttributes">The message attributes.</param>
        /// <param name="customHeaders">The custom headers.</param>
        public static MapField<string, string> AddCustomHeaders(this MapField<string, string> messageAttributes, IDictionary<string, object> customHeaders)
        {
            foreach (var (key, value) in customHeaders)
            {
                if (value != null)
                {
                    messageAttributes.Add(key, value.ToString());
                }
            }

            return messageAttributes;
        }

        /// <summary>
        /// Gets the custom headers.
        /// </summary>
        /// <param name="messageAttributes">The message attributes.</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetCustomHeaders(this MapField<string, string> messageAttributes)
        {
            var returnValue = new Dictionary<string, object>();

            foreach (var (key, value) in messageAttributes)
            {
                returnValue.TryAdd(key, value);
            }

            return returnValue;
        }
    }
}