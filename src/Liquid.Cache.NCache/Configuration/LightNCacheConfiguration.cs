using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Liquid.Cache.NCache.Configuration
{
    /// <summary>
    /// NCache Cache Configuration Class.
    /// </summary>
    /// <seealso>
    ///     <cref>Liquid.Core.Configuration.ILightConfiguration{Liquid.Cache.NCache.Configuration.NCacheSettings}</cref>
    /// </seealso>
    /// <seealso>
    ///     <cref>Liquid.Configuration.AppSetting</cref>
    /// </seealso>
    [ExcludeFromCodeCoverage]
    [LiquidConfigurationSection("cache:nCache")]
    public class LightNCacheConfiguration : LightConfiguration, ILightConfiguration<NCacheSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightNCacheConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public LightNCacheConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public NCacheSettings Settings => GetConfigurationSection<NCacheSettings>();
    }
}
