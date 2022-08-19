﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Liquid.Core.Localization.Entities
{
    /// <summary>
    /// Resource value class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class LocalizationValue
    {
        /// <summary>
        /// Gets or sets the resource value.
        /// </summary>
        /// <value>
        /// The object value.
        /// </value>
        [JsonPropertyName("value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the channels.
        /// </summary>
        /// <value>
        /// The channels.
        /// </value>
        [JsonPropertyName("channels")]
        public string Channels { get; set; }

        /// <summary>
        /// Gets the channels as enumerable.
        /// </summary>
        /// <value>
        /// The channels list.
        /// </value>
        [JsonIgnore]
        public IEnumerable<string> ChannelsList
        {
            get
            {
                if (string.IsNullOrEmpty(Channels)) { return new List<string>(); }
                return Channels.ToLower().Split(';');
            }
        }
    }
}