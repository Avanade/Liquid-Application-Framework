using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Liquid.Cache.Ignite.Configuration
{
    /// <summary>
    /// Apache Ignite Cache Configuration Class.
    /// </summary>
    /// <seealso>
    ///     <cref>Liquid.Core.Configuration.ILightConfiguration{Liquid.Cache.Ignite.Configuration.IgniteSettings}</cref>
    /// </seealso>
    /// <seealso>
    ///     <cref>Liquid.Configuration.AppSetting</cref>
    /// </seealso>
    [ExcludeFromCodeCoverage]
    [ConfigurationSection("igniteCache")]
    public class LightIgniteCacheConfiguration : LightConfiguration, ILightConfiguration<IgniteCacheSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightIgniteCacheConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public LightIgniteCacheConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public IgniteCacheSettings Settings => GetConfigurationSection<IgniteCacheSettings>();
    }
}
