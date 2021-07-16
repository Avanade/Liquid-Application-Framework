using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
        [JsonProperty("items")]
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