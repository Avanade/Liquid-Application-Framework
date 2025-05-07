using Azure;
using Azure.AI.OpenAI;
using Liquid.Core.Entities;
using Liquid.GenAi.OpenAi.Settings;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.ClientModel.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.GenAi.OpenAi
{
    ///<inheritdoc/>
    public class OpenAiClientFactory : IOpenAiClientFactory
    {
        private readonly IOptions<OpenAiOptions> _settings;
        private readonly List<ClientDictionary<ChatClient>> _openAiClients;

        /// <summary>
        /// Initialize a new instance of <see cref="OpenAiClientFactory"/>
        /// </summary>
        /// <param name="settings">Connection options.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public OpenAiClientFactory(IOptions<OpenAiOptions> settings)
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
                throw new KeyNotFoundException($"No settings found for client ID: {clientId}");
            }

            var clientDefinition = _openAiClients.Where(x => x.ClientId == clientId)?.MinBy(x => x.Executions);

            if (clientDefinition != null)
            {
                clientDefinition.Executions++;
                return clientDefinition.Client;
            }

            var response = CreateClient(settings, clientId);

            if (response == null)
            {
                throw new KeyNotFoundException($"No settings found for client ID: {clientId}");
            }

            return response;
        }

        private ChatClient? CreateClient(List<OpenAiSettings>? settings, string clientId)
        {
            if (settings == null || settings.Count == 0)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            ChatClient? client = null;

            foreach (var setting in settings)
            {
                var options = new AzureOpenAIClientOptions();

                options.RetryPolicy = new ClientRetryPolicy(setting.MaxRetries);

                AzureOpenAIClient azureClient = new(new Uri(setting.Url), new AzureKeyCredential(setting.Key), options);

                client = azureClient.GetChatClient(setting.DeploymentName);

                _openAiClients.Add(new ClientDictionary<ChatClient>(setting.ClientId, client));
            }

            var result = _openAiClients.Where(x => x.ClientId == clientId).MinBy(x => x.Executions);

            return result?.Client;
        }
    }
}
