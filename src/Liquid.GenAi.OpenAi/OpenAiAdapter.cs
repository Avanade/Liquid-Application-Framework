using Liquid.Core.Entities;
using Liquid.Core.GenAi;
using Liquid.Core.GenAi.Entities;
using Liquid.Core.GenAi.Enums;
using Liquid.Core.GenAi.Settings;
using OpenAI.Chat;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.GenAi.OpenAi
{
    ///<inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class OpenAiAdapter : ILiquidGenAi
    {

        private readonly IOpenAiClientFactory _factory;
        /// <summary>
        /// Initialize a new instance of <see cref="OpenAiAdapter"/>
        /// </summary>
        /// <param name="factory">Open IA client Factory.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public OpenAiAdapter(IOpenAiClientFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        ///<inheritdoc/>
        public async Task<ChatCompletionResult> FunctionCalling(LiquidChatMessages messages, List<FunctionBody> functions, CompletionsOptions settings)
        {
            var client = _factory.GetOpenAIClient(settings.ClientId);

            var requestMessages = new List<ChatMessage>();

            messages.Messages.ForEach(m => requestMessages.Add(MapChatRequestMessage(m)));

            var option = MapChatCompletionOptions(settings);

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
        public async Task<ChatCompletionResult> CompleteChatAsync(string content, string prompt, CompletionsOptions settings, LiquidChatMessages chatHistory = null)
        {
            var client = _factory.GetOpenAIClient(settings.ClientId);

            var messages = GetChatMessagesAsync(content, prompt, chatHistory);

            var option = MapChatCompletionOptions(settings);

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
        public async Task<ChatCompletionResult> CompleteChatAsync(LiquidChatMessages messages, CompletionsOptions settings, List<FunctionBody> functions = null, LiquidChatMessages chatHistory = null)
        {
            var client = _factory.GetOpenAIClient(settings.ClientId);

            var requestMessages = new List<ChatMessage>();

            messages.Messages.ForEach(m => requestMessages.Add(MapChatRequestMessage(m)));

            var option = MapChatCompletionOptions(settings);

            var responseWithoutStream = await client.CompleteChatAsync(requestMessages, option);

            if (functions != null)
            {
                functions.ForEach(f => option.Tools.Add(ChatTool.CreateFunctionTool(f.Name, f.Description, f.Parameters)));
            }

            if (chatHistory?.Messages != null && chatHistory.Messages.Count > 0)
            {
                foreach (var message in chatHistory.Messages)
                {
                    requestMessages.Add(MapChatRequestMessage(message));
                }
            }

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

        /// <summary>
        /// get chat messages for a chat completions request.
        /// </summary>
        /// <param name="content">content of the user message</param>
        /// <param name="prompt">prompt message</param>
        /// <param name="chatHistory">chat context messages</param>
        private static List<ChatMessage> GetChatMessagesAsync(string content, string prompt, LiquidChatMessages? chatHistory = null)
        {
            var messages = new List<ChatMessage>
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
        private static ChatMessage MapChatRequestMessage(LiquidChatMessage message)
        {
            ChatMessage? chatRequestMessage = null;
            switch (message.Role.ToLower())
            {
                case "system":
                    chatRequestMessage = new SystemChatMessage(CreateContent(message));
                    break;
                case "assistant":
                    chatRequestMessage = new AssistantChatMessage(CreateContent(message));
                    break;
                case "user":
                    chatRequestMessage = new UserChatMessage(CreateContent(message));
                    break;
                default:
                    break;
            }

            if (chatRequestMessage == null)
            {
                throw new ApplicationException($"The folowing message is invalid: {message}");
            }

            return chatRequestMessage;
        }

        private static IEnumerable<ChatMessageContentPart> CreateContent(LiquidChatMessage message)
        {
            var messageList = message.Content.ToList();

            var content = new List<ChatMessageContentPart>();

            messageList.ForEach(x => content.Add(x.Kind == LiquidContentKind.Text ?
                ChatMessageContentPart.CreateTextPart(x.Text) :
                ChatMessageContentPart.CreateImagePart(x.ImageUri)));

            return content;
        }

        /// <summary>
        /// Return a chat completions options based on the chat completions settings.
        /// </summary>
        /// <param name="settings">Chat completions settings</param>
        /// <returns></returns>
        private static ChatCompletionOptions MapChatCompletionOptions(CompletionsOptions settings)
        {
            return new ChatCompletionOptions()
            {
                Temperature = settings.Temperature,
                MaxOutputTokenCount = settings.MaxTokens,
                TopP = settings.TopP,
                FrequencyPenalty = settings.FrequencyPenalty,
                PresencePenalty = settings.PresencePenalty

            };
        }
    }
}
