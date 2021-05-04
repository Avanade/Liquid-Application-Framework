using Liquid.Core.Base;

namespace Liquid.Core.Telemetry
{
    /// <summary>
    /// Telemetry Type Enumeration.
    /// </summary>
    /// <seealso cref="Liquid.Core.Base.Enumeration" />
    public class TelemetryType : Enumeration
    {
        /// <summary>
        /// The information telemetry type.
        /// </summary>
        public static readonly TelemetryType Information = new TelemetryType(0, "Information");

        /// <summary>
        /// The warning telemetry type.
        /// </summary>
        public static readonly TelemetryType Warning = new TelemetryType(1, "Warning");

        /// <summary>
        /// The error telemetry type.
        /// </summary>
        public static readonly TelemetryType Error = new TelemetryType(2, "Error");

        /// <summary>
        /// The metric telemetry type.
        /// </summary>
        public static readonly TelemetryType Metric = new TelemetryType(3, "Metric");

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryType"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="displayName">The display name.</param>
        public TelemetryType(int value, string displayName) : base(value, displayName)
        {
        }
    }
}