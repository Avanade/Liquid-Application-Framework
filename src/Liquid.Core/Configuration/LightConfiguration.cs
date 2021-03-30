using System;
using System.Linq;
using System.Reflection;
using Liquid.Core.Utils;
using Microsoft.Extensions.Configuration;

namespace Liquid.Core.Configuration
{
    /// <summary>
    /// Light Configuration Class.
    /// </summary>
    public abstract class LightConfiguration
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="NotImplementedException">The {nameof(ConfigurationSectionAttribute)} attribute decorator must be added to configuration class.</exception>
        protected LightConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            if (!GetType().GetCustomAttributes(typeof(ConfigurationSectionAttribute), true).Any())
            {
                throw new NotImplementedException($"The {nameof(ConfigurationSectionAttribute)} attribute decorator must be added to configuration class.");
            }
        }

        /// <summary>
        /// Gets the configuration section.
        /// </summary>
        /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
        /// <returns></returns>
        public TConfiguration GetConfigurationSection<TConfiguration>()
        {
            var sectionAttribute = GetType().GetCustomAttribute<ConfigurationSectionAttribute>(true);
            var result = _configuration.GetSection(sectionAttribute.SectionName);

            return result.Value.ParseJson<TConfiguration>();
        }
    }
}