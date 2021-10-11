using Liguid.Repository.Configuration;
using Liquid.Core.Interfaces;
using Liquid.Repository.Configuration;
using Liquid.Repository.Mongo.Configuration;
using Liquid.Repository.Mongo.Exceptions;
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
    public class MongoClientFactoryTests
    {
        private IMongoClientFactory _sut;
        internal static MongoDbRunner _runner;
        internal const string _databaseName = "TestDatabase";
        private IOptionsSnapshot<DatabaseSettings> _databaseSettings;

    [SetUp]
        protected void SetContext()
        {
            _runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: false);

            var options = new DatabaseSettings()
            {
                DatabaseName = _databaseName,
                ConnectionString = _runner.ConnectionString
            };

            _databaseSettings = Substitute.For<IOptionsSnapshot<DatabaseSettings>>();
            _databaseSettings.Get(_databaseName).Returns(options);

            var options2 = new DatabaseSettings()
            {
                DatabaseName = $"{_databaseName}-2",
                ConnectionString = _runner.ConnectionString
            };
            _databaseSettings.Get($"{_databaseName}-2").Returns(options2);

            _sut = new MongoClientFactory(_databaseSettings);
        }


        [TearDown]
        public void DisposeResources() 
        {
            _databaseSettings = null;
            _sut = null;
            _runner.Dispose();
            _runner = null;
        }

        [Test]
        public void MongoClientFactory_WhenSettingsDoesntExists_ThrowException()
        {
            IOptionsSnapshot<DatabaseSettings> nullConfig = null;
            Assert.Throws<LiquidDatabaseSettingsDoesNotExistException>(() => new MongoClientFactory(nullConfig));
        }

        [Test]
        public void GetClient_WhenDatabaseIdsExists_ClientCreated()
        {
            var result = _sut.GetClient(_databaseName);
            Assert.IsNotNull(result.GetDatabase(_databaseName));

            result = _sut.GetClient($"{_databaseName}-2");
            Assert.IsNotNull(result.GetDatabase($"{_databaseName}-2"));
        }

        [Test]
        public void GetClient_WhenDatabaseIdDoesntExists_ThrowException()
        {
            Assert.Throws<LiquidDatabaseSettingsDoesNotExistException>(() => _sut.GetClient("anyDbId"));
        }
    }
}
