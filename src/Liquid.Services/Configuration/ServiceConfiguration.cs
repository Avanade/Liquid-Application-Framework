using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Liquid.Services.Configuration
{
    /// <summary>
    /// Service Configuration Class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [ConfigurationSection("services")]
    public class ServiceConfiguration : LightConfiguration, ILightServiceConfiguration<LightServiceSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ServiceConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <inheritdoc/>
        public LightServiceSetting Settings => throw new System.NotImplementedException();

        /// <inheritdoc/>
        public LightServiceSetting GetSettings(string configurationSection)
        {
            return GetConfigurationSection<LightServiceSetting>($"liquid:services:{configurationSection}");
        }
    }
}