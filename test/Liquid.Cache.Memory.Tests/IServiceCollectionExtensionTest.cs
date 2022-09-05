using Liquid.Cache.Memory.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Linq;
using Xunit;

namespace Liquid.Cache.Memory.Tests
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
        public void AddLiquidMemoryDistributedCache_WhenWithTelemetryTrue_GetServicesReturnLiqudCache()
        {
            SetCollection();
            _sut.AddLogging();
            _sut.AddLiquidMemoryDistributedCache(options =>
            {
                options.SizeLimit = 3000;
            }, true);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<ILiquidCache>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidCache) && x.Lifetime == ServiceLifetime.Scoped));
            Assert.NotNull(_sut.FirstOrDefault(x => x.ImplementationType == typeof(MemoryDistributedCache)));

        }

        [Fact]
        public void AddLiquidMemoryDistributedCache_WhenWithTelemetryfalse_GetServicesReturnLiqudCache()
        {
            SetCollection();
            _sut.AddLiquidMemoryDistributedCache(options =>
            {
                options.SizeLimit = 3000;
            }, false);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<ILiquidCache>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidCache) && x.Lifetime == ServiceLifetime.Scoped));
            Assert.NotNull(_sut.FirstOrDefault(x => x.ImplementationType == typeof(MemoryDistributedCache)));

        }
    }
}