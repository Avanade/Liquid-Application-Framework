using Google.Cloud.PubSub.V1;
using Grpc.Core;
using Liquid.Messaging.Gcp.Configuration;

namespace Liquid.Messaging.Gcp.Factories
{
    internal class EmulatedPubSubClientFactory : IPubSubClientFactory
    {
        public PublisherServiceApiClient GetPublisher(PubSubSettings pubSubSettings)
        {
            PublisherServiceApiClient client = new PublisherServiceApiClientBuilder
            {
                Endpoint = pubSubSettings.ConnectionStringOrFile,
                ChannelCredentials = ChannelCredentials.Insecure
            }.Build();

            return client;
        }

        public SubscriberServiceApiClient GetSubscriberServiceApiClient(PubSubSettings pubSubSettings)
        {
            var serviceClientApiBuilder =
                new SubscriberServiceApiClientBuilder
                {
                    Endpoint = pubSubSettings.ConnectionStringOrFile,
                    ChannelCredentials = ChannelCredentials.Insecure,
                };

            return serviceClientApiBuilder.Build();
        }

        public SubscriberClient GetSubscriberClient(PubSubSettings pubSubSettings, SubscriptionName subscriptionName)
        {
            return SubscriberClient.Create(subscriptionName,
                    new SubscriberClient.ClientCreationSettings(serviceEndpoint: pubSubSettings.ConnectionStringOrFile, credentials: ChannelCredentials.Insecure));
        }


    }
}
