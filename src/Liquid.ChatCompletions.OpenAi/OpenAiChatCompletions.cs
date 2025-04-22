using Liquid.Core.Entities;
using Liquid.Core.Interfaces;
using Liquid.Core.Settings;
using OpenAI.Chat;
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

            var requestMessages = new List<OpenAI.Chat.ChatMessage>();

            messages.Messages.ForEach(m => requestMessages.Add(MapChatRequestMessage(m)));

            var option = MapChatCompletionOptions(requestMessages, settings);

            functions.ForEach(f => option.Tools.Add(ChatTool.CreateFunctionTool(f.Name, f.Description, f.Parameters)));

            var responseWithoutStream = await client.CompleteChatAsync(requestMessages, option);
            var response = responseWithoutStream.Value.Content[0].Text;

            var result = new ChatCompletionResult()
            {
                FinishReason = responseWithoutStream.Value.FinishReason.ToString(),
                Content = response,
                Usage = responseWithoutStream.Value.Usage.TotalTokenCount,
                PromptUsage = responseWithoutStream.Value.Usage.InputTokenCount,
                CompletionUsage = responseWithoutStream.Value.Usage.OutputTokenCount,
            };

            return result;
        }

        ///<inheritdoc/>
        public async Task<ChatCompletionResult> ChatCompletions(string content, string prompt, CompletionsSettings settings, ChatMessages? chatHistory = null)
        {
            var client = _factory.GetOpenAIClient(settings.ClientId);

            var messages = GetChatMessagesAsync(content, prompt, chatHistory);

            var option = MapChatCompletionOptions(messages, settings);

            var responseWithoutStream = await client.CompleteChatAsync(messages, option);
            var response = responseWithoutStream.Value.Content[0].Text;

            var result = new ChatCompletionResult()
            {
                FinishReason = responseWithoutStream.Value.FinishReason.ToString(),
                Content = response,
                Usage = responseWithoutStream.Value.Usage.TotalTokenCount,
                PromptUsage = responseWithoutStream.Value.Usage.InputTokenCount,
                CompletionUsage = responseWithoutStream.Value.Usage.OutputTokenCount,
            };

            return result;
        }

        ///<inheritdoc/>
        public async Task<ReadOnlyMemory<float>> GetEmbeddings(string description, string modelName, string clientId)
        {
            //var client = _factory.GetOpenAIClient(clientId);

            //EmbeddingGenerationOptions embeddingsOptions = new(modelName, new string[] { description });

            //var embeddings = await client.(embeddingsOptions);

            //return embeddings.Value.Data[0].Embedding;

            throw new NotImplementedException();
        }


        /// <summary>
        /// get chat messages for a chat completions request.
        /// </summary>
        /// <param name="content">content of the user message</param>
        /// <param name="prompt">prompt message</param>
        /// <param name="chatHistory">chat context messages</param>
        private List<OpenAI.Chat.ChatMessage> GetChatMessagesAsync(string content, string prompt, ChatMessages? chatHistory = null)
        {
            var messages = new List<OpenAI.Chat.ChatMessage>
            {
                new SystemChatMessage(prompt)
            };

            if (chatHistory?.Messages != null && chatHistory.Messages.Count > 0)
            {
                foreach (var message in chatHistory.Messages)
                {
                    messages.Add(MapChatRequestMessage(message));
                }
            }

            messages.Add(new UserChatMessage(content));

            return messages;
        }

        /// <summary>
        /// Return a chat request message based on the role of the message.
        /// </summary>
        /// <param name="message">chat message</param>
        /// <exception cref="ArgumentNullException"></exception>
        private OpenAI.Chat.ChatMessage MapChatRequestMessage(Core.Entities.ChatMessage message)
        {
            OpenAI.Chat.ChatMessage chatRequestMessage = null;
            switch (message.Role.ToLower())
            {
                case "system":
                    chatRequestMessage = new SystemChatMessage(message.Content);
                    break;
                case "assistant":
                    chatRequestMessage = new AssistantChatMessage(message.Content);
                    break;
                case "user":
                    chatRequestMessage = new UserChatMessage(message.Content);
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
        private ChatCompletionOptions MapChatCompletionOptions(List<OpenAI.Chat.ChatMessage> messages, CompletionsSettings settings)
        {
            return new ChatCompletionOptions()
            {
                Temperature = settings.Temperature,
                MaxOutputTokenCount = settings.MaxTokens,
                TopP = settings.NucleusSamplingFactor,
                FrequencyPenalty = settings.FrequencyPenalty,
                PresencePenalty = settings.PresencePenalty

            };
        }
    }
}
