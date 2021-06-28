using Liquid.Core.Implementations;
using Liquid.Core.UnitTests.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using Xunit;

namespace Liquid.Core.UnitTests
{
    public class LiquidConfigurationTest
    {
        private IOptions<MockSettings> _options = Substitute.For<IOptions<MockSettings>>();
        private IConfiguration _config = Substitute.For<IConfiguration>();
        private MockSettings _settings = new MockSettings();

        [Fact]
        public void Ctor_WhenInjectOptions_Success()
        {
            _settings.MyProperty = "options";

            _options.Value.Returns(_settings);

            var sut = new LiquidConfiguration<MockSettings>(_options, _config);

            Assert.True(sut.Settings.MyProperty == _settings.MyProperty);
        }

        [Fact]
        public void Ctor_WhenInjectLiquidConfiguration_Success()
        {
            _settings.MyProperty = "Liquid";

            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json");

            _config = builder.Build();

            var sut = new LiquidConfiguration<MockSettings>(_options, _config);

            Assert.True(sut.Settings.MyProperty == _settings.MyProperty);
        }

        [Fact]
        public void Ctor_WhenInjectSectionDoesntExists_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LiquidConfiguration<MockSettings>(_options, _config));
        }

        [Fact]
        public void Ctor_WhenEntityHasNoAttribute_Success()
        {
            _settings.MyProperty = "Liquid";

            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json");

            _config = builder.Build();

            var options = Substitute.For<IOptions<MockNoAttributeSettings>>();

            var sut = new LiquidConfiguration<MockNoAttributeSettings>(options, _config);

            Assert.True(sut.Settings.MyProperty == _settings.MyProperty);
        }
    }
}
