using Azure;
using Azure.AI.OpenAI;
using Azure.Core;
using Liquid.Core.Entities;
using Liquid.Core.Settings;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;

namespace Liquid.ChatCompletions.OpenAi
{
    ///<inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class OpenAiClientFactory : IOpenAiClientFactory
    {
        private readonly IOptions<GenAiOptions> _settings;
        private readonly List<ClientDictionary<ChatClient>> _openAiClients;

        /// <summary>
        /// Initialize a new instance of <see cref="OpenAiClientFactory"/>
        /// </summary>
        /// <param name="settings">Connection options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public OpenAiClientFactory(IOptions<GenAiOptions> settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _openAiClients = new List<ClientDictionary<ChatClient>>();
        }

        ///<inheritdoc/>
        public ChatClient GetOpenAIClient(string clientId)
        {
            var settings = _settings.Value.Settings.Where(x => x.ClientId == clientId).ToList();

            if (settings.Count == 0)
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            var clientDefinition = _openAiClients.Where(x => x.ClientId == clientId)?.MinBy(x => x.Executions);

            if (clientDefinition == null)
            {
                return CreateClient(settings);
            }
            else
            {
                clientDefinition.Executions++;
                return clientDefinition.Client;
            }
        }

        private ChatClient CreateClient(List<GenAiSettings> settings)
        {
            if (settings == null || settings.Count == 0)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            ChatClient? client = null;        

            foreach (var setting in settings)
            {               

                var options = new AzureOpenAIClientOptions();

                //options.Retry.MaxRetries = setting.MaxRetries;
                //options.Retry.Delay = TimeSpan.FromSeconds(setting.RetryMinDelay);
                //options.Retry.Mode = setting.ExponentialBackoff ? RetryMode.Exponential : RetryMode.Fixed;
                //options.Retry.MaxDelay = TimeSpan.FromSeconds(setting.RetryMaxDelay);


                AzureOpenAIClient azureClient = new(new Uri(setting.Url), new AzureKeyCredential(setting.Key), options);

                client = azureClient.GetChatClient("");

                _openAiClients.Add(new ClientDictionary<ChatClient>(setting.ClientId, client));
            }

            return client;
        }
    }
}
