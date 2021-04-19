using Liquid.Core.Telemetry;
using Liquid.Repository.MongoDb.Tests.Mock;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Liquid.Repository.MongoDb.Tests
{
    class MongoDbRepositoryTests
    {
        private ILightTelemetryFactory _telemetryFactory;
        private ILightTelemetry _lightTelemetry;
        private IMongoDbDataContext _dbDataContext;
        private ILightRepository<TestEntity, int> _sut;
        private TestEntity _entity;
        internal static string _databaseName = "IntegrationTest";
        private IMongoCollection<TestEntity> _collection;

        [SetUp]
        protected void SetContext()
        {
            _telemetryFactory = Substitute.For<ILightTelemetryFactory>();
            _lightTelemetry = Substitute.For<ILightTelemetry>();
            _telemetryFactory.GetTelemetry().Returns(_lightTelemetry);

            _entity = new TestEntity()
            {
                CreatedDate = DateTime.Now,
                Active = true,
                Id = 1234,
                MockTitle = "test"
            };


            _dbDataContext = Substitute.For<IMongoDbDataContext>();

            _collection = GetCollection();

            _dbDataContext.Database.GetCollection<TestEntity>("TestEntities")
                .Returns(_collection);

            _sut = new TestRepository(_telemetryFactory, _dbDataContext);
        }

        private IMongoCollection<TestEntity> GetCollection()
        {
            var listEntities = new List<TestEntity>() { _entity };

            var result = Substitute.For<IMongoCollection<TestEntity>>();

            var cursor = new AsyncCursorMock<TestEntity>(listEntities.AsEnumerable());

            result.FindAsync<TestEntity>(Arg.Any<FilterDefinition<TestEntity>>()).Returns(cursor);

            return result;
        }


        [Test]
        public async Task ValidateCollection_WhenCollectionExists_Success()
        {
            await _sut.AddAsync(_entity);

            _dbDataContext.Database.Received(2).GetCollection<TestEntity>("TestEntities");
        }

        [Test]
        public async Task AddAsync_WhenCollectionExists_EntityAddedSuccessfuly()
        {

            await _sut.AddAsync(_entity);

            _dbDataContext.Database.Received(2).GetCollection<TestEntity>("TestEntities");

            await _collection.Received(1).InsertOneAsync(_entity);

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());
        }

        [Test]
        public async Task GetAllAsync_WhenCollectionExists_ReturnNoItens()
        {

            var result = await _sut.GetAllAsync();

            _dbDataContext.Database.Received(2).GetCollection<TestEntity>("TestEntities");

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

            Assert.IsTrue(result.Count() == 1);
            Assert.AreEqual(result.FirstOrDefault(), _entity);

        }
        [Test]
        public async Task FindByIdAsync_WhenCollectionExists_ReturnNull()
        {

            var result = await _sut.FindByIdAsync(1234);

            _dbDataContext.Database.Received(2).GetCollection<TestEntity>("TestEntities");

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

            Assert.IsTrue(result == _entity);

        }
        [Test]
        public async Task RemoveAsync_WhenCollectionExists_ReturnNull()
        {
            await _sut.RemoveAsync(_entity);

            _dbDataContext.Database.Received(2).GetCollection<TestEntity>("TestEntities");

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

            await _collection.Received().DeleteOneAsync(Arg.Any<FilterDefinition<TestEntity>>());

        }

        [Test]
        public async Task UpdateAsync_WhenCollectionExists_ReplaceOneAsyncExecutedSuccessfuly()
        {

            await _sut.UpdateAsync(_entity);

            _dbDataContext.Database.Received().GetCollection<TestEntity>("TestEntities");

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

            await _collection.Received().ReplaceOneAsync(Arg.Any<FilterDefinition<TestEntity>>(), _entity, Arg.Any<ReplaceOptions>());

        }

        [Test]
        public async Task WhereAsync_WhenCollectionExists_ReturnNull()
        {
            var result = await _sut.WhereAsync(e => e.Id.Equals(_entity.Id));

            _dbDataContext.Database.Received().GetCollection<TestEntity>("TestEntities");

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

            Assert.IsTrue(result.Count() == 1);
            Assert.AreEqual(result.FirstOrDefault(), _entity);
        }
    }
}
