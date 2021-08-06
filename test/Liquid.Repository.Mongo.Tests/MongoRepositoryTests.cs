using Liquid.Repository.Mongo.Tests.Mock;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Liquid.Repository.Mongo.Tests
{
    [ExcludeFromCodeCoverage]
    class MongoRepositoryTests
    {
        private IMongoDataContext<TestEntity> _dbDataContext;
        private ILiquidRepository<TestEntity, int> _sut;
        private TestEntity _entity;
        internal static string _databaseName = "IntegrationTest";
        private IMongoCollection<TestEntity> _collection;

        [SetUp]
        protected void SetContext()
        {

            _entity = new TestEntity()
            {
                CreatedDate = DateTime.Now,
                Active = true,
                Id = 1234,
                MockTitle = "test"
            };


            _dbDataContext = Substitute.For<IMongoDataContext<TestEntity>>();

            _dbDataContext.Settings.Returns(new Attributes.MongoAttribute("TestEntities", "id", _databaseName));

            IClientSessionHandle handle = null;

            _dbDataContext.ClientSessionHandle.Returns(handle, handle);

            _collection = GetCollection();

            _dbDataContext.Database.GetCollection<TestEntity>("TestEntities")
                .Returns(_collection);

            _sut = new MongoRepository<TestEntity, int>(_dbDataContext);
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

            _dbDataContext.Database.Received(1).GetCollection<TestEntity>("TestEntities");
        }

        [Test]
        public async Task AddAsync_WhenActionIsSuccessful_CallInsertOneMethod()
        {

            await _sut.AddAsync(_entity);

            _dbDataContext.Database.Received(1).GetCollection<TestEntity>("TestEntities");

            await _collection.Received(1).InsertOneAsync(_entity);
        }

        [Test]
        public void AddAsync_WhenClientThrowsError_ThrowException()
        {
            _collection.When(o => o.InsertOneAsync(Arg.Any<TestEntity>())).Do((call) => throw new Exception());

            var test = _sut.AddAsync(_entity);

            Assert.ThrowsAsync<Exception>(() => test);
        }

        [Test]
        public async Task FindAllAsync_WhenCollectionExists_ReturnItens()
        {
            var result = await _sut.FindAllAsync();

            _dbDataContext.Database.Received(1).GetCollection<TestEntity>("TestEntities");

            Assert.NotNull(result);
            Assert.AreEqual(result.FirstOrDefault(), _entity);

        }

        [Test]
        public void FindAllAsync_WhenClientThrowsError_ThrowException()
        {
            _dbDataContext.Database.When(o => o.GetCollection<TestEntity>(Arg.Any<string>())).Do((call) => throw new Exception());

            var test = _sut.FindAllAsync();

            Assert.ThrowsAsync<Exception>(() => test);
        }

        [Test]
        public async Task FindByIdAsync_WhenItemExists_ReturnItem()
        {

            var result = await _sut.FindByIdAsync(1234);

            _dbDataContext.Database.Received(1).GetCollection<TestEntity>("TestEntities");

            Assert.IsTrue(result == _entity);

        }

        [Test]
        public void FindByIdAsync_WhenClientThrowsError_ThrowException()
        {
            _collection.When(o => o.FindAsync<TestEntity>(Arg.Any<FilterDefinition<TestEntity>>())).Do((call) => throw new Exception());

            var test = _sut.FindByIdAsync(1234);

            Assert.ThrowsAsync<Exception>(() => test);

        }             

        [Test]
        public async Task RemoveByIdAsync_WhenActionIsSuccessful_CallDeleteOneMethod()
        {
            await _sut.RemoveByIdAsync(_entity.Id);

            _dbDataContext.Database.Received(1).GetCollection<TestEntity>("TestEntities");

            await _collection.Received().DeleteOneAsync(Arg.Any<FilterDefinition<TestEntity>>());

        }

        [Test]
        public void RemoveByIdAsync_WhenClientThrowsError_ThrowException()
        {
            _collection.When(o => o.DeleteOneAsync(Arg.Any<FilterDefinition<TestEntity>>())).Do((call) => throw new Exception());

            var test = _sut.RemoveByIdAsync(_entity.Id);

            Assert.ThrowsAsync<Exception>(() => test);
        }

        [Test]
        public async Task UpdateAsync_WhenActionIsSuccessful_CallReplaceOneMethod()
        {

            await _sut.UpdateAsync(_entity);

            _dbDataContext.Database.Received().GetCollection<TestEntity>("TestEntities");

            await _collection.Received().ReplaceOneAsync(Arg.Any<FilterDefinition<TestEntity>>(), _entity, Arg.Any<ReplaceOptions>());

        }

        [Test]
        public void UpdateAsync_WhenClientThrowsError_ThrowException()
        {
            _collection.When(o => o.ReplaceOneAsync(Arg.Any<FilterDefinition<TestEntity>>(), _entity, Arg.Any<ReplaceOptions>())).Do((call) => throw new Exception());

            var test = _sut.UpdateAsync(_entity);

            Assert.ThrowsAsync<Exception>(() => test);
        }

        [Test]
        public async Task WhereAsync_WhenItensExists_ReturnItens()
        {
            var result = await _sut.WhereAsync(e => e.Id.Equals(_entity.Id));

            _dbDataContext.Database.Received().GetCollection<TestEntity>("TestEntities");

            Assert.NotNull(result);
            Assert.AreEqual(result.FirstOrDefault(), _entity);
        }

        [Test]
        public void WhereAsync_WhenClientThrowsError_ThrowException()
        {
            _collection.When(o => o.FindAsync<TestEntity>(Arg.Any<FilterDefinition<TestEntity>>())).Do((call) => throw new Exception());

            var test = _sut.WhereAsync(e => e.Id.Equals(_entity.Id));

            Assert.ThrowsAsync<Exception>(() => test);
        }
    }
}
