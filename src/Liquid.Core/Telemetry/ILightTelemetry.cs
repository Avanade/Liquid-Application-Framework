namespace Liquid.Core.Telemetry
{
    /// <summary>
    /// Liquid 
    /// </summary>
    public interface ILightTelemetry
    {
        /// <summary>
        /// Adds the context into telemetry stack.
        /// </summary>
        /// <param name="context">The context.</param>
        void AddContext(string context);

        /// <summary>
        /// Removes the context from the telemetry stack.
        /// </summary>
        /// <param name="context">The context.</param>
        void RemoveContext(string context);

        /// <summary>
        /// Adds data to telemetry.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="telemetryData">The telemetry data.</param>
        void AddTelemetry(TelemetryType type, object telemetryData);

        /// <summary>
        /// Adds information data to telemetry.
        /// </summary>
        /// <param name="telemetryData">The telemetry data.</param>
        void AddInformationTelemetry(object telemetryData);

        /// <summary>
        /// Adds warning data to telemetry.
        /// </summary>
        /// <param name="telemetryData">The telemetry data.</param>
        void AddWarningTelemetry(object telemetryData);

        /// <summary>
        /// Adds error data to telemetry.
        /// </summary>
        /// <param name="telemetryData">The telemetry data.</param>
        void AddErrorTelemetry(object telemetryData);

        /// <summary>
        /// Starts a stop watch to measure the execution time of a process related to the key.
        /// </summary>
        /// <param name="key">The key.</param>
        void StartTelemetryStopWatchMetric(string key);

        /// <summary>
        /// Stops the execution time measurement and collects the data to add to telemetry.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="telemetryData">The telemetry data.</param>
        void CollectTelemetryStopWatchMetric(string key, object telemetryData = null);

        /// <summary>
        /// Flushes all telemetries to log and clear the telemetry data.
        /// </summary>
        void Flush();
    }
}