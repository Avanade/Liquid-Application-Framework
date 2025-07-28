using Liquid.Core.GenAi.Enums;
using System;
using System.Collections.Generic;
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
        public LiquidMessageRole Role { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidChatMessage"/> class with the specified role.
        /// </summary>
        /// <param name="role">The role associated with the chat message. This value cannot be null or empty.</param>
        public LiquidChatMessage(LiquidMessageRole role)
        {
            Role = role;
        }

        /// <summary>
        /// The contents of the message.
        /// </summary>
        public LiquidChatContent[] Content { get; set; } = Array.Empty<LiquidChatContent>();


        /// <summary>
        /// Adds a text content item to the current collection of chat contents.
        /// </summary>
        /// <remarks>This method appends the specified text as a new content item to the existing
        /// collection. If the collection is initially null, it initializes the collection with the new content
        /// item.</remarks>
        /// <param name="text">The text content to add. Cannot be null, empty, or consist solely of whitespace.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="text"/> is null, empty, or consists only of whitespace.</exception>
        public void AddContent(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Text content cannot be null or empty.", nameof(text));
            }
            var content = new LiquidChatContent
            {
                Kind = LiquidContentKind.Text,
                Text = text
            };
            if (Content == null)
            {
                Content = new[] { content };
            }
            else
            {
                var contentList = new List<LiquidChatContent>(Content) { content };
                Content = contentList.ToArray();
            }
        }

        /// <summary>
        /// Adds an image content to the current collection of chat contents.
        /// </summary>
        /// <remarks>If the content collection is initially empty, this method initializes it with the new
        /// image content. Otherwise, it appends the image content to the existing collection.</remarks>
        /// <param name="imageUri">The URI of the image to be added. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="imageUri"/> is <see langword="null"/>.</exception>
        public void AddContent(Uri imageUri)
        {
            if (imageUri == null)
            {
                throw new ArgumentNullException(nameof(imageUri), "Image URI cannot be null.");
            }
            var content = new LiquidChatContent
            {
                Kind = LiquidContentKind.Image,
                ImageUri = imageUri
            };
            if (Content == null)
            {
                Content = new[] { content };
            }
            else
            {
                var contentList = new List<LiquidChatContent>(Content) { content };
                Content = contentList.ToArray();
            }
        }
    }
}