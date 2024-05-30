using EphemeralMongo;
using Liquid.Core.Implementations;
using Liquid.Repository.Mongo.Extensions;
using Liquid.Repository.Mongo.Tests.Mock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
        private IMongoRunner _runner;

        [SetUp]
        public void Setup()
        {
            var options = new MongoRunnerOptions
            {
                UseSingleNodeReplicaSet = true,
                AdditionalArguments = "--quiet",
                KillMongoProcessesWhenCurrentProcessExits = true
            };

            _runner = MongoRunner.Run(options);

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
                                        .AddInMemoryCollection(mongoEntityConfiguration).Build();

            _services.AddSingleton<IConfiguration>(_configuration);
        }

        [TearDown]
        public void DisposeResources()
        {
            _configuration = null;
            _serviceProvider = null;
            _services = null;
            _runner.Dispose();
            _runner = null;
        }

        [Test]
        public void AddLiquidMongoRepository_WhenAdded_ServicesIsFilledForTestEntity()
        {
            _services.AddLiquidMongoRepository<TestEntity, int>(_entityConfigurationSectionName);
            _serviceProvider = _services.BuildServiceProvider();
            ClassicAssert.IsNotNull(_serviceProvider.GetService<IMongoDataContext<TestEntity>>());
            ClassicAssert.IsNotNull(_serviceProvider.GetService<ILiquidRepository<TestEntity, int>>());
        }
        
    }
}
