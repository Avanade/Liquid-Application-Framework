using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Liquid.Messaging.Aws.Extensions
{
    /// <summary>
    /// Sqs Extensions
    /// </summary>
    internal static class SqsExtensions
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
                    messageAttributes.TryAdd(key, new MessageAttributeValue {DataType = "String", StringValue = value.ToString()});
                }
            }

            return messageAttributes;
        }

        /// <summary>
        /// Gets the custom headers.
        /// </summary>
        /// <param name="messageAttributes">The message attributes.</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetCustomHeaders(this Dictionary<string, MessageAttributeValue> messageAttributes)
        {
            var returnValue = new Dictionary<string, object>();

            foreach (var (key, value) in messageAttributes)
            {
                returnValue.TryAdd(key, value?.StringValue);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the queue URL asynchronous.
        /// </summary>
        /// <param name="sqsClient">The SQS client.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Queue name should not be blank.</exception>
        /// <exception cref="System.InvalidOperationException">Could not retrieve the URL for the queue '{queueName}' as it does not exist or you do not have access to it.</exception>
        public static async Task<string> GetAwsQueueUrlAsync(this IAmazonSQS sqsClient, string queueName)
        {
            if (string.IsNullOrEmpty(queueName)) { throw new ArgumentException("Queue name should not be blank."); }

            try
            {
                var response = await sqsClient.GetQueueUrlAsync(queueName);
                return response.QueueUrl;
            }
            catch (QueueDoesNotExistException ex)
            {
                throw new InvalidOperationException($"Could not retrieve the URL for the queue '{queueName}' as it does not exist or you do not have access to it.", ex);
            }
        }
    }
}