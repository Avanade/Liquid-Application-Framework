using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.GenAi.Entities
{
    /// <summary>
    /// Context message associated with a chat completions request.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LiquidChatMessage
    {
        /// <summary>
        /// The chat role associated with this message.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// The contents of the message.
        /// </summary>
        public LiquidChatContent[] Content { get; set; }
    }
}