using System;
using System.Collections.Generic;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;

namespace Liquid.Services.Grpc.Extensions
{
    /// <summary>
    /// Telemetry Extensions Class.
    /// </summary>
    internal static class TelemetryExtensions
    {
        /// <summary>
        /// Collects the GRPC call information.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="telemetry">The telemetry.</param>
        /// <param name="key">The key.</param>
        /// <param name="target">The target.</param>
        /// <param name="response">The response.</param>
        public static void CollectGrpcCallInformation<TResponse>(this ILightTelemetry telemetry, string key, object target, TResponse response)
        {
            var request = new Dictionary<string, string>();
            target?.GetType().GetFields().Each(field => request.TryAdd(field.Name, field.GetValue(target).ToJson()));

            var trackingObject = new
            {
                title = "Liquid.Services.Grpc Log Information",
                request = new { data = request, size = $"{Math.Round(Convert.ToDecimal(request.ToBytes().GetKbSize()), 2)} Kb" },
                response = new { data = response.ToJson(), size = $"{Math.Round(Convert.ToDecimal(response.ToJsonBytes().ToBytes().GetKbSize()), 2)} Kb" }
            };

            telemetry.CollectTelemetryStopWatchMetric(key, trackingObject);
        }
    }
}