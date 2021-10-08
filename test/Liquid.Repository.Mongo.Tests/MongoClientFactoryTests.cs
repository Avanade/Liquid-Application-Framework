using Liguid.Repository.Configuration;
using Liquid.Core.Interfaces;
using Liquid.Repository.Configuration;
using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Exceptions;
using Microsoft.Extensions.Configuration;
using Mongo2Go;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    public class MongoClientFactoryTests
    {
        private IMongoClientFactory _sut;
        internal static MongoDbRunner _runner;
        internal const string _databaseId = "IntegrationTest";
        internal const string _databaseName = "TestDatabase";
        internal const string _configurationSectionName = "MyMongoDbSettings";
        private IConfiguration _configurationSection;

        [SetUp]
        protected void SetContext()
        {
            _runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: false);

            var mongoDatabaseConfiguration = new Dictionary<string, string>
            {
                {$"{_configurationSectionName}:{_databaseId}:DatabaseName", _databaseName},
                {$"{_configurationSectionName}:{_databaseId}:ConnectionString", _runner.ConnectionString},
                {$"{_configurationSectionName}:{_databaseId}-2:DatabaseName", $"{_databaseName}-2"},
                {$"{_configurationSectionName}:{_databaseId}-2:ConnectionString", _runner.ConnectionString}
            };

            _configurationSection = new ConfigurationBuilder()
                                        .AddInMemoryCollection(mongoDatabaseConfiguration)
                                        .Build()
                                        .GetSection(_configurationSectionName);

            _sut = new MongoClientFactory(_configurationSection);
        }


        [TearDown]
        public void DisposeResources() 
        {
            _configurationSection = null;
            _sut = null;
            _runner.Dispose();
            _runner = null;
        }

        [Test]
        public void MongoClientFactory_WhenSettingsDoesntExists_ThrowException()
        {
            IConfiguration nullConfig = null;
            Assert.Throws<LiquidDatabaseSettingsDoesNotExistException>(() => new MongoClientFactory(nullConfig));
        }

        [Test]
        public void GetClient_WhenDatabaseIdsExists_ClientCreated()
        {
            var result = _sut.GetClient(_databaseId);
            Assert.IsNotNull(result.GetDatabase(_databaseName));

            result = _sut.GetClient($"{_databaseId}-2");
            Assert.IsNotNull(result.GetDatabase($"{_databaseName}-2"));
        }

        [Test]
        public void GetClient_WhenDatabaseIdDoesntExists_ThrowException()
        {
            Assert.Throws<LiquidDatabaseSettingsDoesNotExistException>(() => _sut.GetClient("anyDbId"));
        }
    }
}
