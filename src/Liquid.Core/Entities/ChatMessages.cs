
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.Entities
{
    /// <summary>
    /// The object of context messages associated with a chat completions request
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ChatMessages
    {
        /// <summary>
        /// The collection of context messages associated with a chat completions request.
        /// </summary>
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
    /// <summary>
    /// Context message associated with a chat completions request.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ChatMessage
    {
        /// <summary>
        /// The chat role associated with this message.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// The contents of the message.
        /// </summary>
        public string Content { get; set; }
    }

}