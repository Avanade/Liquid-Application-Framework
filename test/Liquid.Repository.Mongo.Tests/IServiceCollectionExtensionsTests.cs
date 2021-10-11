using Liguid.Repository.Configuration;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Repository.Configuration;
using Liquid.Repository.Mongo.Configuration;
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
using Liquid.Repository.Mongo.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    public class IServiceCollectionExtensionsTests
    {
        internal const string _databaseConfigurationSectionName = "MyMongoDbSettings";
        internal const string _entityConfigurationSectionName = "MyMongoEntityOptions";
        internal const string _databaseName = "TestDatabase";

        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;
        private IConfiguration _mongoEntityOptions;
        private IConfiguration _databaseSettings;
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
                {$"{_databaseConfigurationSectionName}:{_databaseName}-2:DatabaseName", $"{_databaseName}-2"},
                {$"{_databaseConfigurationSectionName}:{_databaseName}-2:ConnectionString", _runner.ConnectionString}
            };

            var mongoEntityConfiguration = new Dictionary<string, string>
            {
                {$"{_entityConfigurationSectionName}:TestEntity:DatabaseName", _databaseName},
                {$"{_entityConfigurationSectionName}:TestEntity:CollectionName", "TestEntities"},
                {$"{_entityConfigurationSectionName}:TestEntity:ShardKey", "id"},
                {$"{_entityConfigurationSectionName}:AnotherTestEntity:DatabaseName", _databaseName},
                {$"{_entityConfigurationSectionName}:AnotherTestEntity:CollectionName", "AnotherTestEntities"},
                {$"{_entityConfigurationSectionName}:AnotherTestEntity:ShardKey", "id"}
            };

            var builder = new ConfigurationBuilder()
                                        .AddInMemoryCollection(mongoDatabaseConfiguration)
                                        .AddInMemoryCollection(mongoEntityConfiguration)
                                        .Build();

            _mongoEntityOptions = builder.GetSection(_entityConfigurationSectionName);
            _databaseSettings = builder.GetSection(_databaseConfigurationSectionName);
        }

        [TearDown]
        public void DisposeResources()
        {
            _mongoEntityOptions = null;
            _serviceProvider = null;
            _services = null;
            _runner.Dispose();
            _runner = null;
        }

        [Test]
        public void AddLiquidMongoRepository_WhenMongoOptionsAreNull_ThrowsException()
        {
            Assert.Throws<LiquidDatabaseSettingsDoesNotExistException>(() => _services.AddLiquidMongoRepository<TestEntity, int>(null, options => { options.DatabaseName = _databaseName; options.CollectionName = "TestEntities"; options.ShardKey = "id"; }));
            Assert.Throws<MongoEntityOptionsSettingsDoesNotExistException>(() => _services.AddLiquidMongoRepository<TestEntity, int>(_databaseSettings, null));
        }

        [Test]
        public void AddLiquidMongoRepository_WhenAdded_ServicesIsFilledForTestEntity()
        {
            _services.AddLiquidMongoRepository<TestEntity, int>(_databaseSettings, options => { options.DatabaseName = _databaseName; options.CollectionName = "TestEntities"; options.ShardKey = "id"; });
            _serviceProvider = _services.BuildServiceProvider();
            Assert.IsNotNull(_serviceProvider.GetService<IMongoDataContext<TestEntity>>());
            Assert.IsNotNull(_serviceProvider.GetService<ILiquidRepository<TestEntity, int>>());
        }

        [Test]
        public void AddLiquidMongoRepositories_WhenMongoOptionsConfigurationDoesntExist_ThrowsException()
        {
            Assert.Throws<LiquidDatabaseSettingsDoesNotExistException>(() => _services.AddLiquidMongoRepositories(null, _mongoEntityOptions));
            Assert.Throws<MongoEntityOptionsSettingsDoesNotExistException>(() => _services.AddLiquidMongoRepositories(_databaseSettings, null));
        }

        [Test]
        public void AddLiquidMongoRepositories_WhenAdded_ServicesIsFilledForTestEntities()
        {
            _services.AddLiquidMongoRepositories(_databaseSettings, _mongoEntityOptions);
            _serviceProvider = _services.BuildServiceProvider();
            Assert.IsNotNull(_serviceProvider.GetService<IMongoDataContext<TestEntity>>());
            Assert.IsNotNull(_serviceProvider.GetService<ILiquidRepository<TestEntity, int>>());
            Assert.IsNotNull(_serviceProvider.GetService<IMongoDataContext<AnotherTestEntity>>());
            Assert.IsNotNull(_serviceProvider.GetService<ILiquidRepository<AnotherTestEntity, int>>());
        }
    }
}
