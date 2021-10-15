using Liquid.Repository.Configuration;
using Mongo2Go;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    public class MongoClientFactoryTests
    {
        private IMongoClientFactory _sut;
        internal static MongoDbRunner _runner;
        internal const string _databaseName = "TestDatabase";
        private DatabaseSettings _correctDatabaseSettings;
        private DatabaseSettings _wrongDatabaseSettings;

        [SetUp]
        protected void SetContext()
        {
            _runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: false);

            _correctDatabaseSettings = new DatabaseSettings()
            {
                DatabaseName = _databaseName,
                ConnectionString = _runner.ConnectionString
            };

            _wrongDatabaseSettings = new DatabaseSettings()
            {
                DatabaseName = $"{_databaseName}-2",
                ConnectionString = "incorrect connection string"
            };

            _sut = new MongoClientFactory();
        }


        [TearDown]
        public void DisposeResources() 
        {
            _correctDatabaseSettings = null;
            _sut = null;
            _runner.Dispose();
            _runner = null;
        }

        [Test]
        public void MongoClientFactory_WhenSettingsIsNull_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.GetClient(null));
        }

        [Test]
        public void GetClient_WhenDatabaseIdsExists_ClientCreated()
        {
            var result = _sut.GetClient(_correctDatabaseSettings);
            Assert.IsNotNull(result.GetDatabase(_databaseName));
        }

        [Test]
        public void GetClient_WhenDatabaseSettingsIsWrong_ThrowException()
        {
            Assert.Throws<MongoDB.Driver.MongoConfigurationException>(() => _sut.GetClient(_wrongDatabaseSettings));
        }
    }
}
