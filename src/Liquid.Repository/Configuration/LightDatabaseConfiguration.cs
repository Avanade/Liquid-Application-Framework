using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace Liguid.Repository.Configuration
{
    /// <summary>
    /// Database configuration class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [ConfigurationSection("databases")]
    public class LightDatabaseConfiguration : LightConfiguration, ILightConfiguration<List<LightConnectionSettings>>
    {
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public List<LightConnectionSettings> Settings => GetConfigurationSection<List<LightConnectionSettings>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LightDatabaseConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public LightDatabaseConfiguration(IConfiguration configuration) : base(configuration)
        { 
        }
    }
}