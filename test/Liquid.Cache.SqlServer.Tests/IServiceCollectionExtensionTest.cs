using Liquid.Cache.SqlServer.Extensions.DependencyInjection;
using Liquid.Core.Interfaces;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace Liquid.Cache.SqlServer.Tests
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
        public void AddLiquidSqlServerDistributedCache_WhenWithTelemetryTrue_GetServicesReturnLiqudCache()
        {
            SetCollection();
            _sut.AddLogging();
            _sut.AddLiquidSqlServerDistributedCache(options =>
            {
                options.ConnectionString = "DistCache_ConnectionString";
                options.SchemaName = "dbo";
                options.TableName = "TestCache";
            }, true);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<ILiquidCache>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidCache) && x.Lifetime == ServiceLifetime.Scoped));
            Assert.NotNull(_sut.FirstOrDefault(x => x.ImplementationType == typeof(SqlServerCache)));

        }

        [Fact]
        public void AddLiquidSqlServerDistributedCache_WhenWithTelemetryfalse_GetServicesReturnLiqudCache()
        {
            SetCollection();
            _sut.AddLiquidSqlServerDistributedCache(options =>
            {
                options.ConnectionString = "DistCache_ConnectionString";
                options.SchemaName = "dbo";
                options.TableName = "TestCache";
            }, false);

            var provider = _sut.BuildServiceProvider();

            Assert.NotNull(provider.GetService<ILiquidCache>());
            Assert.NotNull(_sut.FirstOrDefault(x => x.ServiceType == typeof(ILiquidCache) && x.Lifetime == ServiceLifetime.Scoped));
            Assert.NotNull(_sut.FirstOrDefault(x => x.ImplementationType == typeof(SqlServerCache)));

        }
    }
}