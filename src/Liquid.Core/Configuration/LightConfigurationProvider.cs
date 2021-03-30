using System;
using System.IO;
using Liquid.Core.Utils;
using Microsoft.Extensions.Configuration;

namespace Liquid.Core.Configuration
{
    /// <summary>
    /// Light Configuration Provider.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Configuration.ConfigurationProvider" />
    public class LightConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// The filepath
        /// </summary>
        private readonly string _filepath;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightConfigurationProvider"/> class.
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <exception cref="Liquid.Core.Configuration.LightFileConfigurationException"></exception>
        public LightConfigurationProvider(string filepath = null)
        {
            _filepath = filepath ?? GetAppSettingsFilePath();
            if (!File.Exists(_filepath)) throw new LightFileConfigurationException(_filepath);
        }

        /// <summary>
        /// Loads (or reloads) the data for this provider.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public override void Load()
        {
            try
            {
                using var stream = File.Open(_filepath, FileMode.Open);
                Data = JsonConfigurationFileParser.Parse(stream);
            }
            catch (Exception ex)
            {
                throw new LightFileConfigurationException(_filepath, ex);
            }
        }

        /// <summary>
        /// Gets the application settings file path.
        /// </summary>
        /// <returns></returns>
        private string GetAppSettingsFilePath()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            var filepath =
                environment.IsNotNullOrEmpty() &&
                File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}appsettings.{environment}.json") ?
                    $"{AppDomain.CurrentDomain.BaseDirectory}appsettings.{environment}.json" :
                    $"{AppDomain.CurrentDomain.BaseDirectory}appsettings.json";


            return filepath;
        }
    }
}