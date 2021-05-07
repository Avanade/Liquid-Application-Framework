using Google.Cloud.PubSub.V1;
using Liquid.Messaging.Gcp.Configuration;

namespace Liquid.Messaging.Gcp.Factories
{
    /// <summary>
    /// Pub/Sub Client Factory Interface.
    /// </summary>
    public interface IPubSubClientFactory
    {
        /// <summary>
        /// Gets the publisher.
        /// </summary>
        /// <param name="pubSubSettings">The pub sub settings.</param>
        /// <returns></returns>
        PublisherServiceApiClient GetPublisher(PubSubSettings pubSubSettings);

        /// <summary>
        /// Gets the subscriber Service Api Client.
        /// </summary>
        /// <param name="pubSubSettings">The pub sub settings.</param>
        /// <returns></returns>
        SubscriberServiceApiClient GetSubscriberServiceApiClient(PubSubSettings pubSubSettings);

        /// <summary>
        /// Gets the subscriber client.
        /// </summary>
        /// <param name="pubSubSettings">The pub sub settings.</param>
        /// <param name="subscriptionName">Name of the subscription.</param>
        /// <returns></returns>
        SubscriberClient GetSubscriberClient(PubSubSettings pubSubSettings, SubscriptionName subscriptionName);
    }
}
