using Google.Api.Gax.Grpc;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using Grpc.Core;
using Liquid.Messaging.Gcp.Configuration;
using System;
using System.IO;

namespace Liquid.Messaging.Gcp.Factories
{
    /// <summary>
    /// Default Pub/Sub Client Factory, create true clients for Pub/Sub service.
    /// </summary>
    /// <seealso cref="Liquid.Messaging.Gcp.Factories.IPubSubClientFactory" />
    internal class DefaultPubSubClientFactory : IPubSubClientFactory
    {
        /// <inheritdoc/>
        public PublisherServiceApiClient GetPublisher(PubSubSettings pubSubSettings)
        {
            var credentialPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pubSubSettings.ConnectionStringOrFile);
            var credentials = CallCredentials.FromInterceptor(GoogleAuthInterceptors.FromCredential(GoogleCredential.FromFile(credentialPath)));
            var serviceClientApiBuilder = new PublisherServiceApiClientBuilder
            {
                Settings = new PublisherServiceApiSettings
                {
#pragma warning disable CS0618
                    CallSettings = CallSettings.FromCallCredentials(credentials)
#pragma warning restore CS0618
                }
            };

            return serviceClientApiBuilder.Build();
        }

        /// <inheritdoc/>
        public SubscriberServiceApiClient GetSubscriberServiceApiClient(PubSubSettings pubSubSettings)
        {
            var credentialPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pubSubSettings.ConnectionStringOrFile);
            var credentials = CallCredentials.FromInterceptor(GoogleAuthInterceptors.FromCredential(GoogleCredential.FromFile(credentialPath)));
            var serviceClientApiBuilder =
                new SubscriberServiceApiClientBuilder
                {
                    Settings = new SubscriberServiceApiSettings
                    {
#pragma warning disable CS0618
                        CallSettings = CallSettings.FromCallCredentials(credentials)
#pragma warning restore CS0618
                    }
                };

            return serviceClientApiBuilder.Build();
        }

        /// <inheritdoc/>
        public SubscriberClient GetSubscriberClient(PubSubSettings pubSubSettings, SubscriptionName subscriptionName)
        {
            var credentialPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pubSubSettings.ConnectionStringOrFile);
            var credentials = CallCredentials.FromInterceptor(GoogleAuthInterceptors.FromCredential(GoogleCredential.FromFile(credentialPath)));

            return SubscriberClient.Create(subscriptionName,
                    new SubscriberClient.ClientCreationSettings(subscriberServiceApiSettings:
                    new SubscriberServiceApiSettings
                    {
#pragma warning disable CS0618
                        CallSettings = CallSettings.FromCallCredentials(credentials)
#pragma warning restore CS0618
                    }));
        }


    }
}
