using Azure.AI.OpenAI;

namespace Liquid.ChatCompletions.OpenAi
{
    /// <summary>
    /// Provide <see cref="OpenAIClient"/> generator methods.
    /// </summary>
    public interface IOpenAiClientFactory
    {
        /// <summary>
        /// Provide a instance of <see cref="OpenAIClient"/> with conection started.
        /// </summary>
        /// <param name="clientId">OpenAI connections alias.</param>
        OpenAIClient GetOpenAIClient(string clientId);
    }
}
