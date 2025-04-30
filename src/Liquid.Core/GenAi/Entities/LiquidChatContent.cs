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
        public LiquidContentKind Kind { get; set; }

        public string Text { get; set; }

        public Uri ImageUri { get; set; }

    }
}