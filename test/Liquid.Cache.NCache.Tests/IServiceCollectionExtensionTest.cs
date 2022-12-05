using Liquid.Cache.NCache.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace Liquid.Cache.NCache.Tests
{
    public class IServiceCollectionExtensionTest
    {
        private IServiceCollection _sut;
        private IConfiguration _configProvider = Substitute.For<IConfiguration>();
        private IConfigurationSection _configurationSection = Substitute.For<IConfigurationSection>();

        private void SetCollection()
        {
            _configProvider.GetSection(Arg.Any<string>()).Returns(_configurationSection);
            _sut = new ServiceCollection();
            _sut.AddSingleton(_configProvider);
        }

        [Fact]
        public void AddLiquidNCacheDistributedCache_WhenWithTelemetryTrue_GetServicesReturnLiqudCache()
        {
            SetCollection();
            _sut.AddLogging();
            _sut.AddLiquidNCacheDistributedCache(configuration =>
            {
                configuration.CacheName = "myCache";
                configuration.EnableLogs = true;
                configuration.ExceptionsEnabled = true;
            }, true);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<ILiquidCache>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidCache) && x.Lifetime == ServiceLifetime.Scoped));

        }

        [Fact]
        public void AddLiquidNCacheDistributedCache_WhenWithTelemetryfalse_GetServicesReturnLiqudCache()
        {
            SetCollection();
            _sut.AddLiquidNCacheDistributedCache(configuration =>
            {
                configuration.CacheName = "myCache";
                configuration.EnableLogs = true;
                configuration.ExceptionsEnabled = true;
            }, false);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<ILiquidCache>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidCache) && x.Lifetime == ServiceLifetime.Scoped));

        }
    }
}