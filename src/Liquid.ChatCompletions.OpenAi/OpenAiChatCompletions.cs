using Liquid.Core.Entities;
using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using Azure.AI.OpenAI;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.ChatCompletions.OpenAi
{
    ///<inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class OpenAiChatCompletions : ILiquidChatCompletions
    {

        private readonly IOpenAiClientFactory _factory;
        /// <summary>
        /// Initialize a new instance of <see cref="OpenAiChatCompletions"/>
        /// </summary>
        /// <param name="factory">Open IA client Factory.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public OpenAiChatCompletions(IOpenAiClientFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        ///<inheritdoc/>
        public async Task<ChatCompletionResult> FunctionCalling(ChatMessages messages, List<FunctionBody> functions, CompletionsSettings settings)
        {
            var client = _factory.GetOpenAIClient(settings.ClientId);

            var requestMessages = new List<ChatRequestMessage>();

            messages.Messages.ForEach(m => requestMessages.Add(MapChatRequestMessage(m)));

            var option = MapChatCompletionOptions(requestMessages, settings);

            functions.ForEach(f => option.Functions.Add(GetDefinition(f)));

            var responseWithoutStream = await client.GetChatCompletionsAsync(option, new CancellationToken());

            var response = responseWithoutStream.Value.Choices[0].Message.FunctionCall == null ?
               null : responseWithoutStream.Value.Choices[0].Message.FunctionCall.Arguments;

            var result = new ChatCompletionResult()
            {
                FinishReason = responseWithoutStream.Value.Choices[0]?.FinishReason?.ToString(),
                Content = response,
                Usage = responseWithoutStream.Value.Usage.TotalTokens,
                PromptUsage = responseWithoutStream.Value.Usage.PromptTokens,
                CompletionUsage = responseWithoutStream.Value.Usage.CompletionTokens,
            };

            return result;
        }

        ///<inheritdoc/>
        public async Task<ChatCompletionResult> ChatCompletions(string content, string prompt, CompletionsSettings settings, ChatMessages? chatHistory = null)
        {
            var client = _factory.GetOpenAIClient(settings.ClientId);

            var messages = GetChatMessagesAsync(content, prompt, chatHistory);

            var option = MapChatCompletionOptions(messages, settings);

            var responseWithoutStream = await client.GetChatCompletionsAsync(option, new CancellationToken());

            var result = new ChatCompletionResult()
            {
                FinishReason = responseWithoutStream.Value.Choices[0]?.FinishReason?.ToString(),
                Content = responseWithoutStream.Value.Choices[0]?.Message?.Content,
                Usage = responseWithoutStream.Value.Usage.TotalTokens,
                PromptUsage = responseWithoutStream.Value.Usage.PromptTokens,
                CompletionUsage = responseWithoutStream.Value.Usage.CompletionTokens,
            };

            return result;
        }

        ///<inheritdoc/>
        public async Task<ReadOnlyMemory<float>> GetEmbeddings(string description, string modelName, string clientId)
        {
            var client = _factory.GetOpenAIClient(clientId);

            EmbeddingsOptions embeddingsOptions = new(modelName, new string[] { description });

            var embeddings = await client.GetEmbeddingsAsync(embeddingsOptions);

            return embeddings.Value.Data[0].Embedding;
        }

        private FunctionDefinition GetDefinition(FunctionBody function)
        {
            return new FunctionDefinition()
            {
                Name = function.Name,
                Description = function.Description,
                Parameters = function.Parameters,
            };
        }

        /// <summary>
        /// get chat messages for a chat completions request.
        /// </summary>
        /// <param name="content">content of the user message</param>
        /// <param name="prompt">prompt message</param>
        /// <param name="chatHistory">chat context messages</param>
        private List<ChatRequestMessage> GetChatMessagesAsync(string content, string prompt, ChatMessages? chatHistory = null)
        {
            var messages = new List<ChatRequestMessage>
            {
                new ChatRequestSystemMessage(prompt)
            };

            if (chatHistory?.Messages != null && chatHistory.Messages.Count > 0)
            {
                foreach (var message in chatHistory.Messages)
                {
                    messages.Add(MapChatRequestMessage(message));
                }
            }

            messages.Add(new ChatRequestUserMessage(content));

            return messages;
        }

        /// <summary>
        /// Return a chat request message based on the role of the message.
        /// </summary>
        /// <param name="message">chat message</param>
        /// <exception cref="ArgumentNullException"></exception>
        private ChatRequestMessage MapChatRequestMessage(ChatMessage message)
        {
            ChatRequestMessage chatRequestMessage = null;
            switch (message.Role.ToLower())
            {
                case "system":
                    chatRequestMessage = new ChatRequestSystemMessage(message.Content);
                    break;
                case "assistant":
                    chatRequestMessage = new ChatRequestAssistantMessage(message.Content);
                    break;
                case "user":
                    chatRequestMessage = new ChatRequestUserMessage(message.Content);
                    break;
                default:
                    break;
            }

            if (chatRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(chatRequestMessage));
            }

            return chatRequestMessage;
        }

        /// <summary>
        /// Return a chat completions options based on the chat completions settings.
        /// </summary>
        /// <param name="messages">Chat messages </param>
        /// <param name="settings">Chat completions settings</param>
        /// <returns></returns>
        private ChatCompletionsOptions MapChatCompletionOptions(List<ChatRequestMessage> messages, CompletionsSettings settings)
        {
            return new ChatCompletionsOptions(settings.DeploymentName, messages)
            {
                Temperature = settings.Temperature,
                MaxTokens = settings.MaxTokens,
                NucleusSamplingFactor = settings.NucleusSamplingFactor,
                FrequencyPenalty = settings.FrequencyPenalty,
                PresencePenalty = settings.PresencePenalty,
            };
        }
    }
}
