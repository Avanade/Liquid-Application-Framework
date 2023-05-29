using Liquid.Cache.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Linq;
using Xunit;

namespace Liquid.Cache.Tests
{
    public class IServiceCollectionExtensionTests
    {
        private IServiceCollection _sut;
        private IConfiguration _configProvider = Substitute.For<IConfiguration>();
        private IConfigurationSection _configurationSection = Substitute.For<IConfigurationSection>();
        private readonly IDistributedCache _distributedCache = Substitute.For<IDistributedCache>();

        private void SetCollection()
        {
            _configProvider.GetSection(Arg.Any<string>()).Returns(_configurationSection);
            _sut = new ServiceCollection();
            _sut.AddSingleton(_configProvider);
        }

        [Fact]
        public void AddLiquidDistributedCache_WhenWithTelemetryTrue_GetServicesReturnLiqudCache()
        {
            SetCollection();
            _sut.AddSingleton(_distributedCache);
            _sut.AddLogging();
            _sut.AddLiquidDistributedCache(true);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<ILiquidCache>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidCache) && x.Lifetime == ServiceLifetime.Scoped));

        }

        [Fact]
        public void AddLiquidDistributedCache_WhenWithTelemetryfalse_GetServicesReturnLiqudCache()
        {
            SetCollection();
            _sut.AddSingleton(_distributedCache);
            _sut.AddLiquidDistributedCache(false);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<ILiquidCache>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidCache) && x.Lifetime == ServiceLifetime.Scoped));

        }

    }
}
