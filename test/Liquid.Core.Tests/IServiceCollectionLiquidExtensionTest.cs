using Liquid.Core.Extensions.DependencyInjection;
using Liquid.Core.Implementations;
using Liquid.Core.UnitTests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Linq;
using Xunit;

namespace Liquid.Core.Tests
{
    public class IServiceCollectionLiquidExtensionTest
    {
        private IServiceCollection _sut;

        [Fact]
        [System.Obsolete("The extension method AddLiquidTelemetryInterceptor<IMockService, MockInterceptService>() is obsolete, so is this!")]
        public void AddLiquidTelemetryInterceptor_WhenSuccessfullyInjectsInterceptor_GetServiceSuccessfully()
        {
            SetCollection();

            _sut.AddLiquidTelemetryInterceptor<IMockService, MockInterceptService>();

            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(IMockService) && x.Lifetime == ServiceLifetime.Transient));

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IMockService>());
        }

        [Fact]
        public void AddScopedLiquidTelemetry_WhenSuccessfullyInjectsInterceptor_GetServiceSuccessfully()
        {
            SetCollection();

            _sut.AddScopedLiquidTelemetry<IMockService, MockInterceptService>();

            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(IMockService) && x.Lifetime == ServiceLifetime.Scoped));

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IMockService>());
        }

        private void SetCollection()
        {
            _sut = new ServiceCollection();
            _sut.AddSingleton<MockInterceptService>();
            _sut.AddSingleton(Substitute.For<ILogger<LiquidTelemetryInterceptor>>());
        }

        [Fact]
        public void AddSingletonLiquidTelemetry_WhenSuccessfullyInjectsInterceptor_GetServiceSuccessfully()
        {
            SetCollection();

            _sut.AddSingletonLiquidTelemetry<IMockService, MockInterceptService>();

            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(IMockService) && x.Lifetime == ServiceLifetime.Singleton));

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IMockService>());
        }

        [Fact]
        public void AddTransientLiquidTelemetry_WhenSuccessfullyInjectsInterceptor_GetServiceSuccessfully()
        {
            SetCollection();

            _sut.AddTransientLiquidTelemetry<IMockService, MockInterceptService>();

            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(IMockService) && x.Lifetime == ServiceLifetime.Transient));

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IMockService>());
        }
    }
}
