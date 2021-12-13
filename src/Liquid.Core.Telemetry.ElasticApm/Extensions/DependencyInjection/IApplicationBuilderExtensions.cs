using System.Diagnostics.CodeAnalysis;
using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Liquid.Core.Telemetry.ElasticApm.Extensions.DependencyInjection
{
    /// <summary>
    /// Extends <see cref="IApplicationBuilder"/> interface.
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="UseAllElasticApm"/> to the application builder.
        /// </summary>
        /// <param name="builder">Extended application builder.</param>
        /// <param name="configuration"><see cref="IConfiguration"/> implementation.</param>
        public static IApplicationBuilder UseLiquidElasticApm(this IApplicationBuilder builder, IConfiguration configuration)
        {
            if (configuration.HasElasticApmEnabled())
            {
                builder.UseAllElasticApm(configuration);
            }

            return builder;
        }
    }
}
