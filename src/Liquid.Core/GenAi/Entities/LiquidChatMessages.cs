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
    }
}