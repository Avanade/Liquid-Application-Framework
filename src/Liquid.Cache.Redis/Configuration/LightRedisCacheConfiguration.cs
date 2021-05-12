using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Liquid.Cache.Redis.Configuration
{
    /// <summary>
    /// Redis Cache Configuration Class.
    /// </summary>
    /// <seealso>
    ///     <cref>Liquid.Core.Configuration.ILightConfiguration{Liquid.Cache.Redis.Configuration.RedisCacheSettings}</cref>
    /// </seealso>
    /// <seealso>
    ///     <cref>Liquid.Configuration.AppSetting</cref>
    /// </seealso>
    [ExcludeFromCodeCoverage]
    [LiquidConfigurationSection("cache:redis")]
    public class LightRedisCacheConfiguration : LightConfiguration, ILightConfiguration<RedisCacheSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightRedisCacheConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public LightRedisCacheConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public RedisCacheSettings Settings => GetConfigurationSection<RedisCacheSettings>();
    }
}
