using Liguid.Repository.Configuration;
using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Liquid.Repository.MongoDb.UnitTest
{
    public class MongoDbClientFactoryTests
    {
        private IMongoDbClientFactory _sut;
        internal static MongoDbRunner _runner;
        internal static string _databaseName = "IntegrationTest";
        internal static IMongoDatabase _database;

        [SetUp]
        protected void SetContext()
        {
            _runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: false);

            _sut = new MongoDbClientFactory();

        }

        [Test]
        public async Task GetClient_WhenDatabaseNameExists_ClientCreated()
        {
            var options = Options.Create(new LightConnectionSettings()
            {
                ConnectionString = _runner.ConnectionString,
                DatabaseName = _databaseName
            });

            var result = _sut.GetClient(options);

            Assert.IsFalse(result.GetDatabase(_databaseName) is null);
        }
    }
}
