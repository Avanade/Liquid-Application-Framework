using Liguid.Repository.Configuration;
using Liquid.Core.Interfaces;
using Liquid.Repository.Configuration;
using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Exceptions;
using Liquid.Repository.Mongo.Tests.Mock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Mongo2Go;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    public class MongoEntitySettingsFactoryTests
    {
        internal const string _databaseConfigurationSectionName = "MyMongoDbSettings";
        internal const string _entityConfigurationSectionName = "MyMongoEntityOptions:Entities";
        internal const string _databaseName = "TestDatabase";
        internal const string _connectionString = "test connection string";

        private IMongoEntitySettingsFactory _sutCustomConfiguration;
        private IMongoEntitySettingsFactory _sutStandardConfiguration;
        private IConfiguration _configuration;

        [SetUp]
        protected void SetContext()
        {
            var mongoDatabaseConfiguration = new Dictionary<string, string>
            {
                {$"{_databaseConfigurationSectionName}:{_databaseName}:DatabaseName", _databaseName},
                {$"{_databaseConfigurationSectionName}:{_databaseName}:ConnectionString", _connectionString},
            };

            var mongoEntityConfiguration = new Dictionary<string, string>
            {
                {"MyMongoEntityOptions:DefaultDatabaseSettings:DatabaseName", _databaseName},
                {"MyMongoEntityOptions:DefaultDatabaseSettings:ConnectionString", _connectionString},
                {$"{_entityConfigurationSectionName}:TestEntity:CollectionName", "TestEntities"},
                {$"{_entityConfigurationSectionName}:TestEntity:ShardKey", "id"},
                {$"{_entityConfigurationSectionName}:AnotherTestEntity:CollectionName", "AnotherTestEntities"},
                {$"{_entityConfigurationSectionName}:AnotherTestEntity:ShardKey", "id"},
                {$"{_entityConfigurationSectionName}:AnotherTestEntity:DatabaseSettingsSectionName", $"{_databaseConfigurationSectionName}:{_databaseName}"},
                {$"{_entityConfigurationSectionName}:WrongDBSettings:CollectionName", "AnotherTestEntities"},
                {$"{_entityConfigurationSectionName}:WrongDBSettings:ShardKey", "id"},
                {$"{_entityConfigurationSectionName}:WrongDBSettings:DatabaseSettingsSectionName", "WrongSectionName"}
            };

            var liquidStandardConfiguration = new Dictionary<string, string>
            {
                {"Liquid:RepositorySettings:DefaultDatabaseSettings:DatabaseName", _databaseName},
                {"Liquid:RepositorySettings:DefaultDatabaseSettings:ConnectionString", _connectionString},
                {"Liquid:RepositorySettings:Entities:TestEntity:CollectionName", "TestEntities"},
                {"Liquid:RepositorySettings:Entities:TestEntity:ShardKey", "id"},
            };

            _configuration = new ConfigurationBuilder()
                                        .AddInMemoryCollection(mongoDatabaseConfiguration)
                                        .AddInMemoryCollection(mongoEntityConfiguration)
                                        .AddInMemoryCollection(liquidStandardConfiguration)
                                        .Build();

            _sutCustomConfiguration = new MongoEntitySettingsFactory(_configuration, _entityConfigurationSectionName);
            _sutStandardConfiguration = new MongoEntitySettingsFactory(_configuration);
        }

        [Test]
        public void MongoEntitySettingsFactory_WhenConfigurationIsNull_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new MongoEntitySettingsFactory(null));
        }

        [Test]
        public void GetSettings_WhenEntityNameIsNullOrEmpty_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _sutCustomConfiguration.GetSettings(null));
            Assert.Throws<ArgumentNullException>(() => _sutCustomConfiguration.GetSettings(string.Empty));
        }

        [Test]
        public void GetSettings_WhenEntitySettingsDoesntExist_ThrowException()
        {
            Assert.Throws<MongoEntitySettingsDoesNotExistException>(() => _sutStandardConfiguration.GetSettings<AnotherTestEntity>());
            Assert.Throws<MongoEntitySettingsDoesNotExistException>(() => _sutCustomConfiguration.GetSettings("WrongEntityName"));
        }

        [Test]
        public void GetSettings_WhenDatabaseSettingsDoesntExist_ThrowException()
        {
            Assert.Throws<LiquidDatabaseSettingsDoesNotExistException>(() => _sutCustomConfiguration.GetSettings("WrongDBSettings"));
        }

        [Test]
        public void GetSettings_WhenSettingsAreOk_SettingsCreated()
        {
            var result = _sutCustomConfiguration.GetSettings<TestEntity>();
            Assert.IsNotNull(result);
            Assert.AreEqual("TestEntities", result.CollectionName);
            Assert.AreEqual("id", result.ShardKey);
            Assert.AreEqual(_databaseName, result.DatabaseSettings.DatabaseName);
            Assert.AreEqual(_connectionString, result.DatabaseSettings.ConnectionString);

            result = _sutCustomConfiguration.GetSettings<AnotherTestEntity>();
            Assert.IsNotNull(result);
            Assert.AreEqual("AnotherTestEntities", result.CollectionName);
            Assert.AreEqual("id", result.ShardKey);
            Assert.AreEqual($"{_databaseConfigurationSectionName}:{_databaseName}", result.DatabaseSettingsSectionName);
            Assert.AreEqual(_databaseName, result.DatabaseSettings.DatabaseName);
            Assert.AreEqual(_connectionString, result.DatabaseSettings.ConnectionString);

            result = _sutStandardConfiguration.GetSettings<TestEntity>();
            Assert.IsNotNull(result);
            Assert.AreEqual("TestEntities", result.CollectionName);
            Assert.AreEqual("id", result.ShardKey);
            Assert.AreEqual(_databaseName, result.DatabaseSettings.DatabaseName);
            Assert.AreEqual(_connectionString, result.DatabaseSettings.ConnectionString);
        }
    }
}
