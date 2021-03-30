using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Liquid.Core.Telemetry
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TelemetryData
    {
        /// <summary>
        /// Gets or sets the telemetry date time.
        /// </summary>
        /// <value>
        /// The telemetry date time.
        /// </value>
        [JsonProperty("telemetryDateTime")]
        public DateTime TelemetryDateTime { get; set; }

        /// <summary>
        /// Gets or sets the type of telemetry.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty("type")]
        public TelemetryType Type { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("data")]
        public object Data { get; set; }
    }
}