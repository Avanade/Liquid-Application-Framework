namespace Liquid.Core.Telemetry
{
    /// <summary>
    /// Light Telemetry Factory interface.
    /// </summary>
    public interface ILightTelemetryFactory
    {
        /// <summary>
        /// Gets the telemetry.
        /// </summary>
        /// <returns></returns>
        ILightTelemetry GetTelemetry();
    }
}