using Liquid.Core.Telemetry;
using Liquid.Repository.Exceptions;
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
        public async Task AddAsync_WhenActionIsSuccessful_CallInsertOneMethod()
        {

            await _sut.AddAsync(_entity);

            _dbDataContext.Database.Received(2).GetCollection<TestEntity>("TestEntities");

            await _collection.Received(1).InsertOneAsync(_entity);

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());
        }
        [Test]
        public async Task AddAsync_WhenClientThrowsError_ThrowException()
        {
            _collection.When(o => o.InsertOneAsync(Arg.Any<TestEntity>())).Do((call) => throw new Exception());

            var test = _sut.AddAsync(_entity);

            Assert.ThrowsAsync<RepositoryDatabaseContextException>(() => test);
        }

        [Test]
        public async Task GetAllAsync_WhenCollectionExists_ReturnItens()
        {       
            var result = await _sut.GetAllAsync();

            _dbDataContext.Database.Received(2).GetCollection<TestEntity>("TestEntities");

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

            Assert.IsTrue(result.Count() > 0);
            Assert.AreEqual(result.FirstOrDefault(), _entity);

        }

        [Test]
        public async Task GetAllAsync_WhenClientThrowsError_ThrowException()
        {
            _dbDataContext.Database.When(o => o.GetCollection<TestEntity>(Arg.Any<string>())).Do((call) => throw new Exception());

            var test = _sut.GetAllAsync();

            Assert.ThrowsAsync<RepositoryDatabaseContextException>(() => test);
        }

        [Test]
        public async Task FindByIdAsync_WhenItemExists_ReturnItem()
        {

            var result = await _sut.FindByIdAsync(1234);

            _dbDataContext.Database.Received(2).GetCollection<TestEntity>("TestEntities");

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

            Assert.IsTrue(result == _entity);

        }

        [Test]
        public async Task FindByIdAsync_WhenClientThrowsError_ThrowException()
        {
            _collection.When(o => o.FindAsync<TestEntity>(Arg.Any<FilterDefinition<TestEntity>>())).Do((call) => throw new Exception());

            var test = _sut.FindByIdAsync(1234);

            Assert.ThrowsAsync<RepositoryDatabaseContextException>(() => test);

        }

        [Test]
        public async Task RemoveAsync_WhenActionIsSuccessful_CallDeleteOneMethod()
        {
            await _sut.RemoveAsync(_entity);

            _dbDataContext.Database.Received(2).GetCollection<TestEntity>("TestEntities");

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

            await _collection.Received().DeleteOneAsync(Arg.Any<FilterDefinition<TestEntity>>());

        }

        [Test]
        public async Task RemoveAsync_WhenClientThrowsError_ThrowException()
        {
            _collection.When(o => o.DeleteOneAsync(Arg.Any<FilterDefinition<TestEntity>>())).Do((call) => throw new Exception());

            var test = _sut.RemoveAsync(_entity);

            Assert.ThrowsAsync<RepositoryDatabaseContextException>(() => test);
        }

        [Test]
        public async Task UpdateAsync_WhenActionIsSuccessful_CallReplaceOneMethod()
        {

            await _sut.UpdateAsync(_entity);

            _dbDataContext.Database.Received().GetCollection<TestEntity>("TestEntities");

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

            await _collection.Received().ReplaceOneAsync(Arg.Any<FilterDefinition<TestEntity>>(), _entity, Arg.Any<ReplaceOptions>());

        }

        [Test]
        public async Task UpdateAsync_WhenClientThrowsError_ThrowException()
        {
            _collection.When(o => o.ReplaceOneAsync(Arg.Any<FilterDefinition<TestEntity>>(), _entity, Arg.Any<ReplaceOptions>())).Do((call) => throw new Exception());

            var test = _sut.UpdateAsync(_entity);

            Assert.ThrowsAsync<RepositoryDatabaseContextException>(() => test);
        }

        [Test]
        public async Task WhereAsync_WhenItensExists_ReturnItens()
        {
            var result = await _sut.WhereAsync(e => e.Id.Equals(_entity.Id));

            _dbDataContext.Database.Received().GetCollection<TestEntity>("TestEntities");

            _lightTelemetry.Received().AddContext(Arg.Any<string>());
            _lightTelemetry.Received().RemoveContext(Arg.Any<string>());

            Assert.IsTrue(result.Count() > 0);
            Assert.AreEqual(result.FirstOrDefault(), _entity);
        }

        [Test]
        public async Task WhereAsync_WhenClientThrowsError_ThrowException()
        {
            _collection.When(o => o.FindAsync<TestEntity>(Arg.Any<FilterDefinition<TestEntity>>())).Do((call) => throw new Exception());

            var test = _sut.WhereAsync(e => e.Id.Equals(_entity.Id));

            Assert.ThrowsAsync<RepositoryDatabaseContextException>(() => test);
        }
    }
}
