using System;
using Liquid.Core.Telemetry.ElasticApm.Extensions.DependencyInjection;
using Liquid.Core.Telemetry.ElasticApm.Tests.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace Liquid.Core.Telemetry.ElasticApm.Tests
{
    public sealed class IApplicationBuilderExtensionsTests
    {
        private readonly IApplicationBuilder _builder;

        public IApplicationBuilderExtensionsTests()
        {
            _builder = Substitute.For<IApplicationBuilder>();
        }

        [Fact]
        public void UseElasticApmTelemetry_WhenConfigured()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", null);

            var config = new ConfigurationSettings().AddElasticApm();

            // Act
            _builder.UseLiquidElasticApm(config);

            // Assert
            _builder.Received(2);
        }

        [Fact]
        public void UseElasticApmTelemetry_WhenNotConfigured()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", null);

            var config = new ConfigurationBuilder().Build();

            // Act
            _builder.UseLiquidElasticApm(config);

            // Assert
            _builder.Received(1);
        }

        [Fact]
        public void UseElasticApmTelemetry_WhenConfigured_Enabled()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", null);

            var config = new ConfigurationSettings().AddElasticApm(enable: true);

            // Act
            _builder.UseLiquidElasticApm(config);

            // Assert
            _builder.Received(2);
        }

        [Fact]
        public void UseElasticApmTelemetry_WhenConfigured_NotEnabled()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", null);

            var config = new ConfigurationSettings().AddElasticApm(enable: false);

            // Act
            _builder.UseLiquidElasticApm(config);

            // Assert
            _builder.Received(2);
        }

        [Fact]
        public void UseElasticApmTelemetry_WhenEnvironement_Enabled()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", "true");

            var config = new ConfigurationSettings().AddElasticApm();

            // Act
            _builder.UseLiquidElasticApm(config);

            // Assert
            _builder.Received(2);
        }

        [Fact]
        public void UseElasticApmTelemetry_WhenEnvironement_NotEnabled()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", "false");

            var config = new ConfigurationSettings().AddElasticApm();

            // Act
            _builder.UseLiquidElasticApm(config);

            // Assert
            _builder.Received(1);
        }
    }
}
