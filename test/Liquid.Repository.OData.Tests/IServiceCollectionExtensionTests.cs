using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Repository.OData.Extensions;
using Liquid.Repository.OData.Tests.Mock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Repository.OData.Tests
{
    public class IServiceCollectionExtensionTests
    {
        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private IConfiguration _configuration;

        public IServiceCollectionExtensionTests()
        {
            _services = new ServiceCollection();

            var odataEntityConfiguration = new Dictionary<string, string>
            {
                {"MyODataEntityOptions:Settings:1:BaseUrl", "http://localhost:5000"},
                {"MyODataEntityOptions:Settings:1:EntityName", "TestEntity"},
                {"MyODataEntityOptions:Settings:2:BaseUrl", "http://localhost:5000"},
                {"MyODataEntityOptions:Settings:2:EntityName", "AnotherTestEntity"},
            };

            _configuration = new ConfigurationBuilder()
                                        .AddInMemoryCollection(odataEntityConfiguration).Build();

            _services.AddSingleton<IConfiguration>(_configuration);
        }

        [Fact]
        public void AddLiquidODataRepository_WhenAdded_ServicesIsFilledForTestEntity()
        {
            _services.AddLiquidOdataRepository<TestEntity, string>("MyODataEntityOptions", "TestEntity");
            _services.AddLiquidOdataRepository<AnotherTestEntity, int>("MyODataEntityOptions", "AnotherTestEntity");
            _serviceProvider = _services.BuildServiceProvider();
            Assert.NotNull(_serviceProvider.GetService<ILiquidRepository<TestEntity, string>>());
            Assert.NotNull(_serviceProvider.GetService<ILiquidRepository<AnotherTestEntity, int>>());
        }
    }
}
