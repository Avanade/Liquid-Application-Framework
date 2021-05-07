using System;
using System.Linq;
using System.Reflection;
using Liquid.Core.Exceptions;
using Liquid.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        }

        /// <summary>
        /// Gets the configuration section.
        /// </summary>
        /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
        /// <returns></returns>
        public TConfiguration GetConfigurationSection<TConfiguration>()
        {
            if (!GetType().GetCustomAttributes(typeof(ConfigurationSectionAttribute), true).Any())
            {
                throw new LightException($"The {nameof(ConfigurationSectionAttribute)} attribute decorator must be added to configuration class.");
            }
            var sectionAttribute = GetType().GetCustomAttribute<ConfigurationSectionAttribute>(true);
            return _configuration.GetSection(sectionAttribute.SectionName).Get<TConfiguration>();
        }

        /// <summary>
        /// Gets the configuration section.
        /// </summary>
        /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
        /// <param name="configurationSection">The configuration section.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">configurationSection</exception>
        public TConfiguration GetConfigurationSection<TConfiguration>(string configurationSection)
        {
            if (string.IsNullOrEmpty(configurationSection)) throw new ArgumentNullException(nameof(configurationSection));
            return _configuration.GetSection(configurationSection).Get<TConfiguration>();
        }
    }
}