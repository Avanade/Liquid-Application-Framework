using Liquid.Core.Implementations;
using Liquid.Repository.Mongo.Tests.Mock;
using Liquid.Repository.Mongo.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mongo2Go;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    public class IServiceCollectionExtensionsTests
    {
        internal const string _databaseConfigurationSectionName = "MyMongoDbSettings";
        internal const string _entityConfigurationSectionName = "MyMongoEntityOptions:Entities";
        internal const string _databaseName = "TestDatabase";

        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private IConfiguration _configuration;
        private MongoDbRunner _runner;

        [SetUp]
        public void Setup()
        {
            _runner = MongoDbRunner.Start(singleNodeReplSet: true);

            _services = new ServiceCollection();

            _services.AddSingleton(Substitute.For<ILogger<LiquidTelemetryInterceptor>>());

            var mongoDatabaseConfiguration = new Dictionary<string, string>
            {
                {$"{_databaseConfigurationSectionName}:{_databaseName}:DatabaseName", _databaseName},
                {$"{_databaseConfigurationSectionName}:{_databaseName}:ConnectionString", _runner.ConnectionString},
            };

            var mongoEntityConfiguration = new Dictionary<string, string>
            {
                {"MyMongoEntityOptions:DefaultDatabaseSettings:DatabaseName", _databaseName},
                {"MyMongoEntityOptions:DefaultDatabaseSettings:ConnectionString", _runner.ConnectionString},
                {$"{_entityConfigurationSectionName}:TestEntity:CollectionName", "TestEntities"},
                {$"{_entityConfigurationSectionName}:TestEntity:ShardKey", "id"},
                {$"{_entityConfigurationSectionName}:AnotherTestEntity:CollectionName", "AnotherTestEntities"},
                {$"{_entityConfigurationSectionName}:AnotherTestEntity:ShardKey", "id"},
                {$"{_entityConfigurationSectionName}:AnotherTestEntity:DatabaseSettingsSectionName", $"{_databaseConfigurationSectionName}:{_databaseName}"}
            };

            _configuration = new ConfigurationBuilder()
                                        .AddInMemoryCollection(mongoDatabaseConfiguration)
                                        .AddInMemoryCollection(mongoEntityConfiguration)
                                        .Build();

            _services.AddSingleton<IConfiguration>(_configuration);
        }

        [TearDown]
        public void DisposeResources()
        {
            _runner.Dispose();

            _configuration = null;
            _serviceProvider = null;
            _services = null;
            _runner = null;
        }

        [Test]
        public void AddLiquidMongoRepository_WhenAdded_ServicesIsFilledForTestEntity()
        {
            _services.AddLiquidMongoRepository<TestEntity, int>(_entityConfigurationSectionName);
            _serviceProvider = _services.BuildServiceProvider();
            Assert.IsNotNull(_serviceProvider.GetService<IMongoDataContext<TestEntity>>());
            Assert.IsNotNull(_serviceProvider.GetService<ILiquidRepository<TestEntity, int>>());
        }

        [Test]
        public void AddLiquidMongoRepositories_WhenAdded_ServicesIsFilledForTestEntities()
        {
            _services.AddLiquidMongoRepositories(_entityConfigurationSectionName);
            _serviceProvider = _services.BuildServiceProvider();
            Assert.IsNotNull(_serviceProvider.GetService<IMongoDataContext<TestEntity>>());
            Assert.IsNotNull(_serviceProvider.GetService<ILiquidRepository<TestEntity, int>>());
            Assert.IsNotNull(_serviceProvider.GetService<IMongoDataContext<AnotherTestEntity>>());
            Assert.IsNotNull(_serviceProvider.GetService<ILiquidRepository<AnotherTestEntity, int>>());
        }
    }
}
