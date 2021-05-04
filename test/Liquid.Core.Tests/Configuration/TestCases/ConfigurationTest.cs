using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Liquid.Core.Configuration;
using Liquid.Core.Tests.Configuration.Entities;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Liquid.Core.Tests.Configuration.TestCases
{
    /// <summary>
    /// Application configuration test cases class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ConfigurationTest
    {
        private IConfiguration _configurationRoot;
        private ILightConfiguration<CustomSetting> _sut;

        [SetUp]
        public void Setup()
        {
            _configurationRoot = new ConfigurationBuilder()
               .AddLightConfigurationFile()
               .Build();
        }

        /// <summary>
        /// Verifies if can read settings file.
        /// </summary>
        [Test]
        public void Verify_if_can_read_settings_file()
        {
            _sut = new CustomSettingConfiguration(_configurationRoot);
            var settings = _sut.Settings;
            Assert.IsNotNull(settings);
        }

        /// <summary>
        /// Verifies if settings properties types.
        /// </summary>
        [Test]
        public void Verify_if_settings_properties_types()
        {
            _sut = new CustomSettingConfiguration(_configurationRoot);
            var settings = _sut.Settings;
            Assert.IsNotNull(settings);
            Assert.AreEqual(true, settings.Prop1);
            Assert.AreEqual("prop", settings.Prop2);
            Assert.AreEqual(1, settings.Prop3);
            Assert.AreEqual(new DateTime(2020, 10, 10).ToString(CultureInfo.InvariantCulture), settings.Prop4.ToString(CultureInfo.InvariantCulture));
            //Test environment variables
            Assert.AreNotEqual("${TEMP}", settings.Prop5);
            Assert.AreEqual(string.Empty, settings.Prop6);
        }

        [Test]
        public void Verify_if_can_read_from_environment_variables()
        {
            Environment.SetEnvironmentVariable("liquid__customSettings__prop1", "false");
            Environment.SetEnvironmentVariable("liquid__customSettings__prop2", "env");
            Environment.SetEnvironmentVariable("liquid__customSettings__prop3", "2");

            _configurationRoot = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            _sut = new CustomSettingConfiguration(_configurationRoot);
            var settings = _sut.Settings;
            Assert.IsNotNull(settings);
            Assert.AreEqual(false, settings.Prop1);
            Assert.AreEqual("env", settings.Prop2);
            Assert.AreEqual(2, settings.Prop3);
            
            //Clear environment variable
            Environment.SetEnvironmentVariable("liquid:customSettings", null);
        }

        /// <summary>
        /// Verifies the settings errors.
        /// </summary>
        [Test]
        public void Verify_settings_errors()
        {
            Assert.Catch<LightFileConfigurationException>(() => { new ConfigurationBuilder().AddLightConfigurationFile("xxx.txt").Build(); });
            Assert.Catch<NotImplementedException>(() => new WrongCustomSettingConfiguration(null));
        }
    }
}