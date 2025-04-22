using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace Liquid.ChatCompletions.OpenAi
{
    /// <summary>
    /// Provide <see cref="ChatClient"/> generator methods.
    /// </summary>
    public interface IOpenAiClientFactory
    {
        /// <summary>
        /// Provide a instance of <see cref="ChatClient"/> with conection started.
        /// </summary>
        /// <param name="clientId">OpenAI connections alias.</param>
        ChatClient GetOpenAIClient(string clientId);
    }
}
