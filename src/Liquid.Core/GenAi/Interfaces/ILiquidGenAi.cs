using Liquid.Core.Entities;
using Liquid.Core.GenAi.Entities;
using Liquid.Core.GenAi.Settings;
using Liquid.Core.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liquid.Core.GenAi
{
    /// <summary>
    /// This service is the hub of Liquid adapter custom completions for Generative AI.
    /// </summary>
    public interface ILiquidGenAi
    {
        /// <summary>
        /// Get chat completions for provided content and functions.
        /// </summary>
        /// <param name="messages">Context messages associated with chat completions request.</param>
        /// <param name="functions"> A list of functions the model may generate JSON inputs for.</param>
        /// <param name="settings">The options for chat completions request.</param>
        Task<ChatCompletionResult> FunctionCalling(LiquidChatMessages messages, List<FunctionBody> functions, CompletionsOptions settings);

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
        Task<ChatCompletionResult> CompleteChatAsync(string content, string prompt, CompletionsOptions settings, LiquidChatMessages chatHistory = null);

        /// <summary>
        /// Get chat completions for provided chat context messages and functions.
        /// </summary>
        /// <param name="messages">Messages associated with chat completions request.</param>
        /// <param name="functions"> A list of functions the model may generate JSON inputs for.</param>
        /// <param name="chatHistory"> The collection of context messages associated with this chat completions request. </param>
        /// <param name="settings">The options for chat completions request.</param>
        Task<ChatCompletionResult> CompleteChatAsync(LiquidChatMessages messages, CompletionsOptions settings, List<FunctionBody> functions = null, LiquidChatMessages chatHistory = null);
    }
}
