using Liquid.Core.Entities;
using Liquid.Core.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liquid.Core.Interfaces
{
    /// <summary>
    /// This service is the hub of Liquid adapter custom completions for Generative AI.
    /// </summary>
    public interface ILiquidChatCompletions
    {

        /// <summary>
        /// Get chat completions for provided content and functions.
        /// </summary>
        /// <param name="messages">Context messages associated with chat completions request.</param>
        /// <param name="functions"> A list of functions the model may generate JSON inputs for.</param>
        /// <param name="settings">The options for chat completions request.</param>
        Task<ChatCompletionResult> FunctionCalling(ChatMessages messages, List<FunctionBody> functions, CompletionsSettings settings);

        /// <summary>
        /// Get chat completions for provided chat context messages.
        /// </summary>
        /// <param name="content">A request chat message representing an input from the user.</param>
        /// <param name="prompt">A request chat message containing system instructions that influence how the model will generate a chat completions
        /// response.</param>
        /// <param name="settings">The options for chat completions request.</param>
        /// <param name="chatHistory">The collection of context messages associated with this chat completions request.
        /// Typical usage begins with a chat message for the System role that provides instructions for
        /// the behavior of the assistant, followed by alternating messages between the User and
        /// Assistant roles.</param>
        Task<ChatCompletionResult> ChatCompletions(string content, string prompt, CompletionsSettings settings, ChatMessages? chatHistory = null);

        /// <summary>
        /// Return the computed embeddings for a given prompt.
        /// </summary>
        /// <param name="description">Input texts to get embeddings for, encoded as a an array of strings.</param>
        /// <param name="modelName"></param>
        /// <param name="clientId">Client connection alias to use for a chat completions request.
        /// This connection must be configured in application previously <see cref="GenAiSettings"/></param>
        Task<ReadOnlyMemory<float>> GetEmbeddings(string description, string modelName, string clientId);
    }
}
