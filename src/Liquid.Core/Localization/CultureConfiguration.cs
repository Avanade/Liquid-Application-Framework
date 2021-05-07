using Liquid.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Liquid.Core.Localization
{
    /// <summary>
    /// Represents the Culture Configuration inside appsettings.json.
    /// </summary>
    [LiquidConfigurationSection("culture")]
    public class CultureConfiguration : LightConfiguration, ILightConfiguration<CultureSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CultureConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public CultureConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public CultureSettings Settings => GetConfigurationSection<CultureSettings>();
    }

    /// <summary>
    /// Culture Settings
    /// </summary>
    public class CultureSettings
    {
        /// <summary>
        /// Gets the default culture.
        /// </summary>
        /// <value>
        /// The default culture.
        /// </value>
        [JsonProperty("defaultCulture")]
        public string DefaultCulture { get; set; }
    }
}