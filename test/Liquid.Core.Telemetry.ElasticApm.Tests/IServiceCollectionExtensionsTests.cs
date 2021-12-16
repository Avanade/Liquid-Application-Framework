using System;
using Liquid.Core.Telemetry.ElasticApm.Extensions.DependencyInjection;
using Liquid.Core.Telemetry.ElasticApm.Tests.Mocks;
using Liquid.Core.Telemetry.ElasticApm.Tests.Settings;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Liquid.Core.Telemetry.ElasticApm.Tests
{
    public sealed class IServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddElasticApmTelemetry_WhenConfigured()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", null);
            var services = new ServiceCollection().AddLogging();

            var config = new ConfigurationSettings().AddElasticApm();
            services.Configure<ElasticApmSettings>(config.GetSection(nameof(ElasticApmSettings)));

            services.AddLiquidElasticApmTelemetry(config);

            // Act
            using ServiceProvider serviceprovider = services.BuildServiceProvider();
            var behaviour = serviceprovider.GetService<IPipelineBehavior<RequestMock, ResponseMock>>();

            // Assert
            Assert.NotNull(behaviour);
        }

        [Fact]
        public void AddElasticApmTelemetry_WhenNotConfigured()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", null);
            var services = new ServiceCollection().AddLogging();

            var config = new ConfigurationBuilder().Build();

            services.AddLiquidElasticApmTelemetry(config);

            // Act
            using ServiceProvider serviceprovider = services.BuildServiceProvider();
            var behaviour = serviceprovider.GetService<IPipelineBehavior<RequestMock, ResponseMock>>();

            // Assert
            Assert.Null(behaviour);
        }

        [Fact]
        public void AddElasticApmTelemetry_WhenConfigured_Enabled()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", null);
            var services = new ServiceCollection().AddLogging();

            var config = new ConfigurationSettings().AddElasticApm(enable: true);
            services.Configure<ElasticApmSettings>(config.GetSection(nameof(ElasticApmSettings)));

            services.AddLiquidElasticApmTelemetry(config);

            // Act
            using ServiceProvider serviceprovider = services.BuildServiceProvider();
            var behaviour = serviceprovider.GetService<IPipelineBehavior<RequestMock, ResponseMock>>();

            // Assert
            Assert.NotNull(behaviour);
        }

        [Fact]
        public void AddElasticApmTelemetry_WhenConfigured_NotEnabled()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", null);
            var services = new ServiceCollection().AddLogging();

            var config = new ConfigurationSettings().AddElasticApm(enable: false);
            services.Configure<ElasticApmSettings>(config.GetSection(nameof(ElasticApmSettings)));

            services.AddLiquidElasticApmTelemetry(config);

            // Act
            using ServiceProvider serviceprovider = services.BuildServiceProvider();
            var behaviour = serviceprovider.GetService<IPipelineBehavior<RequestMock, ResponseMock>>();

            // Assert
            Assert.Null(behaviour);
        }

        [Fact]
        public void AddElasticApmTelemetry_WhenEnvironement_Enabled()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", "true");

            var services = new ServiceCollection().AddLogging();

            var config = new ConfigurationSettings().AddElasticApm(enable: false);
            services.Configure<ElasticApmSettings>(config.GetSection(nameof(ElasticApmSettings)));

            services.AddLiquidElasticApmTelemetry(config);

            // Act
            using ServiceProvider serviceprovider = services.BuildServiceProvider();
            var behaviour = serviceprovider.GetService<IPipelineBehavior<RequestMock, ResponseMock>>();

            // Assert
            Assert.NotNull(behaviour);
        }

        [Fact]
        public void AddElasticApmTelemetry_WhenEnvironement_NotEnabled()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ELASTIC_APM_ENABLED", "false");

            var services = new ServiceCollection().AddLogging();

            var config = new ConfigurationSettings().AddElasticApm();
            services.Configure<ElasticApmSettings>(config.GetSection(nameof(ElasticApmSettings)));

            services.AddLiquidElasticApmTelemetry(config);

            // Act
            using ServiceProvider serviceprovider = services.BuildServiceProvider();
            var behaviour = serviceprovider.GetService<IPipelineBehavior<RequestMock, ResponseMock>>();

            // Assert
            Assert.Null(behaviour);
        }
    }
}
