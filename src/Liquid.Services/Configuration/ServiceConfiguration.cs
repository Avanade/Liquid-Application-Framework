using System.Collections.Generic;
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
    public class ServiceConfiguration : LightConfiguration, ILightConfiguration<List<LightServiceSetting>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public ServiceConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public List<LightServiceSetting> Settings => GetConfigurationSection<List<LightServiceSetting>>();
    }
}