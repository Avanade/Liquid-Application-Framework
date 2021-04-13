using Liquid.Core.Telemetry;
using System;
using System.Threading.Tasks;

namespace Liquid.Repository.Extensions
{
    /// <summary>
    /// Telemetry Extensions for repository.
    /// </summary>
    public static class TelemetryExtensions
    {
        /// <summary>
        /// Executes the action asynchronous.
        /// </summary>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="contextName">Name of the context.</param>
        /// <param name="action">The action.</param>
        public static async Task ExecuteActionAsync(this ILightTelemetryFactory telemetryFactory, string contextName, Func<Task> action)
        {
            var telemetry = telemetryFactory.GetTelemetry();

            try
            {
                telemetry.AddContext(contextName);
                await action();
            }
            finally
            {
                telemetry.RemoveContext(contextName);
            }
        }
    }
}
