using Liquid.Core.Implementations;
using Liquid.Repository.Mongo.Extensions;
using Liquid.Repository.Mongo.Tests.Mock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mongo2Go;
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
    public class MongoUnitOfWorkFunctionalTests
    {
        private IServiceProvider _serviceProvider;
        private ILiquidUnitOfWork _unitOfWork;
        private ILiquidRepository<TestEntity, int> _sut;
        private MongoDbRunner _runner;

        private readonly TestEntity _entity = new TestEntity()
        {
            CreatedDate = DateTime.Now,
            Active = true,
            Id = 1242,
            MockTitle = "test"
        };
        private readonly TestEntity _updateEntity = new TestEntity()
        {
            CreatedDate = DateTime.Now,
            Active = false,
            Id = 1242,
            MockTitle = "test"
        };

        [SetUp]
        public void Setup()
        {
            _runner = MongoDbRunner.Start(singleNodeReplSet: true);

            var mongoDatabaseConfiguration = new Dictionary<string, string>
            {
                {"Liquid:RepositorySettings:DefaultDatabaseSettings:DatabaseName", "TestDatabase"},
                {"Liquid:RepositorySettings:DefaultDatabaseSettings:ConnectionString", _runner.ConnectionString},
                {"Liquid:RepositorySettings:Entities:TestEntity:CollectionName", "TestCollection"},
                {"Liquid:RepositorySettings:Entities:TestEntity:ShardKey", "Id"}
            };

            var databaseSettings = new ConfigurationBuilder()
                                        .AddInMemoryCollection(mongoDatabaseConfiguration)
                                        .Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(databaseSettings);

            services.AddSingleton(Substitute.For<ILogger<LiquidTelemetryInterceptor>>());

            services.AddLiquidMongoRepository<TestEntity, int>();

            services.AddTransient<ILiquidUnitOfWork, LiquidUnitOfWork>();

            _serviceProvider = services.BuildServiceProvider();

            _unitOfWork = _serviceProvider.GetService<ILiquidUnitOfWork>();

            _sut = _unitOfWork.GetRepository<ILiquidRepository<TestEntity, int>, TestEntity, int>();
        }

        [TearDown]
        public void DisposeResources()
        {
            _unitOfWork.Dispose();
            _runner.Dispose();

            _serviceProvider = null;
            _sut = null;
            _unitOfWork = null;
            _runner = null;
        }

        [Test]
        public async Task AddAsync_WhenCommitTransaction_ItemAdded()
        {
            await _unitOfWork.StartTransactionAsync();

            await _sut.AddAsync(_entity);

            await _unitOfWork.CommitAsync();

            var result = await _sut.FindByIdAsync(1242);

            Assert.NotNull(result);
        }

        [Test]
        public async Task AddAsync_WhenRollbackTransaction_ItemNotInserted()
        {
            await _unitOfWork.StartTransactionAsync();

            await _sut.AddAsync(_entity);

            await _unitOfWork.RollbackTransactionAsync();

            var result = await _sut.FindByIdAsync(1242);

            Assert.Null(result);
        }
        
        [Test]
        public async Task RemoveByIdAsync_WhenCommitTransaction_ItemDeleted()
        {
            await _sut.AddAsync(_entity);

            await _unitOfWork.StartTransactionAsync();

            await _sut.RemoveByIdAsync(_entity.Id);

            await _unitOfWork.CommitAsync();

            var result = await _sut.WhereAsync(e => e.Id.Equals(_entity.Id));

            Assert.IsFalse(result.Any());
        }

        [Test]
        public async Task RemoveByIdAsync_WhenRollbackTransaction_ItemNotDeleted()
        {
            await _sut.AddAsync(_entity);

            await _unitOfWork.StartTransactionAsync();

            await _sut.RemoveByIdAsync(_entity.Id);

            await _unitOfWork.RollbackTransactionAsync();

            var result = await _sut.FindByIdAsync(1242);

            Assert.NotNull(result);
        }

        [Test]
        public async Task UpdateAsync_WhenCommitTransaction_ItemNotDeleted()
        {
            await _sut.AddAsync(_entity);

            await _unitOfWork.StartTransactionAsync();

            await _sut.UpdateAsync(_updateEntity);

            await _unitOfWork.CommitAsync();

            var result = await _sut.FindByIdAsync(1242);

            Assert.AreEqual(_updateEntity.Active, result.Active);
        }

        [Test]
        public async Task UpdateAsync_WhenRollbackTransaction_ItemNotDeleted()
        {
            await _sut.AddAsync(_entity);

            await _unitOfWork.StartTransactionAsync();

            await _sut.UpdateAsync(_updateEntity);

            await _unitOfWork.RollbackTransactionAsync();

            var result = await _sut.FindByIdAsync(1242);

            Assert.AreEqual(_entity.Active, result.Active);
        }
    }
}
