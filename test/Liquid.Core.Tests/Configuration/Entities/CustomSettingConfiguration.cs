using System;
using System.Diagnostics.CodeAnalysis;
using Liquid.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Liquid.Core.Tests.Configuration.Entities
{
    /// <summary>
    /// Test configuration root class.
    /// </summary>
    [LiquidConfigurationSection("customSettings")]
    [ExcludeFromCodeCoverage]
    public class CustomSettingConfiguration : LightConfiguration, ILightConfiguration<CustomSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSettingConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public CustomSettingConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public CustomSetting Settings => GetConfigurationSection<CustomSetting>();
    }

    /// <summary>
    /// Custom setting class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CustomSetting
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Liquid.Core.Tests.Configuration.Entities.CustomSetting"/> is prop1.
        /// </summary>
        /// <value>
        ///   <c>true</c> if prop1; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("prop1")]
        public bool Prop1 { get; set; }

        /// <summary>
        /// Gets or sets the prop2.
        /// </summary>
        /// <value>
        /// The prop2.
        /// </value>
        [JsonProperty("prop2")]
        public string Prop2 { get; set; }

        /// <summary>
        /// Gets or sets the prop3.
        /// </summary>
        /// <value>
        /// The prop3.
        /// </value>
        [JsonProperty("prop3")]
        public int Prop3 { get; set; }

        /// <summary>
        /// Gets or sets the prop4.
        /// </summary>
        /// <value>
        /// The prop4.
        /// </value>
        [JsonProperty("prop4")]
        public DateTime Prop4 { get; set; }

        /// <summary>
        /// Gets or sets the prop5.
        /// </summary>
        /// <value>
        /// The prop5.
        /// </value>
        [JsonProperty("prop5")]
        public string Prop5 { get; set; }

        /// <summary>
        /// Gets or sets the prop6.
        /// </summary>
        /// <value>
        /// The prop6.
        /// </value>
        [JsonProperty("prop6")]
        public string Prop6 { get; set; }
    }

    /// <summary>
    /// Wrong Test configuration root class, without Attribute.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WrongCustomSettingConfiguration : LightConfiguration, ILightConfiguration<CustomSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSettingConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public WrongCustomSettingConfiguration(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public CustomSetting Settings => GetConfigurationSection<CustomSetting>();
    }
}