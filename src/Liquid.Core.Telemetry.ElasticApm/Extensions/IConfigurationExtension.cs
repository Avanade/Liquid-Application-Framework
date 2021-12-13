using System;
using Microsoft.Extensions.Configuration;

namespace Liquid.Core.Telemetry.ElasticApm.Extensions
{
    /// <summary>
    /// Extends <see cref="IConfiguration"/> interface.
    /// </summary>
    internal static class IConfigurationExtension
    {
        /// <summary>
        /// Checks if Elastic APM is enabled.
        /// </summary>
        /// <param name="config">Extended <see cref="IConfiguration"/> instance.</param>
        /// <returns>True if Elastic APM is enabled, otherwise False.</returns>
        internal static bool HasElasticApmEnabled(this IConfiguration config)
        {
            var apmConfigured = config.GetSection("ElasticApm:ServerUrl").Value != null;
            if (apmConfigured)
            {
                var isEnabled = false;
                var apmEnvironment = (Environment.GetEnvironmentVariable("ELASTIC_APM_ENABLED") != null && bool.TryParse(Environment.GetEnvironmentVariable("ELASTIC_APM_ENABLED"), out isEnabled));
                if (!apmEnvironment)
                {
                    return bool.TryParse(config["ElasticApm:Enabled"], out isEnabled) ? isEnabled : apmConfigured;
                }
                return isEnabled;
            }

            return false;
        }
    }
}