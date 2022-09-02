using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Liquid.Core.Localization.Entities
{
    /// <summary>
    /// Resource collection class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class LocalizationCollection
    {
        /// <summary>
        /// Gets or sets the resources.
        /// </summary>
        /// <value>
        /// The resources.
        /// </value>
        [JsonPropertyName("items")]
        public IEnumerable<LocalizationItem> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationCollection"/> class.
        /// </summary>
        public LocalizationCollection()
        {
            Items = new List<LocalizationItem>();
        }
    }
}