using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Liquid.Core.Localization.Entities
{
    /// <summary>
    /// Resource item class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class LocalizationItem
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [JsonProperty("values")]
        public IEnumerable<LocalizationValue> Values { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationItem"/> class.
        /// </summary>
        public LocalizationItem()
        {
            Values = new List<LocalizationValue>();
        }
    }
}