using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Liquid.WebApi.Grpc.Configuration
{
    /// <summary>
    /// Api configuration class.
    /// </summary>
    /// <seealso>
    ///     <cref>Liquid.Core.Configuration.LightConfiguration</cref>
    /// </seealso>
    /// <seealso>
    ///     <cref>Liquid.Core.Configuration.ILightConfiguration{Liquid.WebApi.Http.Configuration.ApiRootSettings}</cref>
    /// </seealso>
    [ExcludeFromCodeCoverage]
    [ConfigurationSection("api")]
    public class ApiConfiguration : LightConfiguration, ILightConfiguration<ApiSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ApiConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public ApiSettings Settings => GetConfigurationSection<ApiSettings>();
    }
}