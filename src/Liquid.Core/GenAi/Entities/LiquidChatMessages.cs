using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.GenAi.Entities
{
    /// <summary>
    /// The object of context messages associated with a chat completions request
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LiquidChatMessages
    {
        /// <summary>
        /// The collection of context messages associated with a chat completions request.
        /// </summary>
        public List<LiquidChatMessage> Messages { get; set; } = new List<LiquidChatMessage>();

        /// <summary>
        /// Adds a message to the collection of chat messages.
        /// </summary>
        /// <param name="message">The chat message to add. Cannot be <see langword="null"/>.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="message"/> is <see langword="null"/>.</exception>
        public void AddMessage(LiquidChatMessage message)
        {
            if (message == null)
            {
                throw new System.ArgumentNullException(nameof(message), "Message cannot be null");
            }
            Messages.Add(message);
        }

    }
}