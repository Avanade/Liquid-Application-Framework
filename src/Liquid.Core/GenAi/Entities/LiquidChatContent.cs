using Liquid.Core.GenAi.Enums;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Core.GenAi.Entities
{
    /// <summary>
    /// The content of the chat message.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LiquidChatContent
    {
        /// <summary>
        /// The kind of content.
        /// </summary>
        public LiquidContentKind Kind { get; set; }

        /// <summary>
        /// The text content of the message.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The image content url of the message.
        /// </summary>
        public Uri ImageUri { get; set; }

    }
}