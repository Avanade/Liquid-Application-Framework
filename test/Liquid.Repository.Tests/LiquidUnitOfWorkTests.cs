using Liquid.Core.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Liquid.Repository.Tests.Mock;
using Liquid.Repository.Exceptions;

namespace Liquid.Repository.Tests
{
    [ExcludeFromCodeCoverage]
    public class LiquidUnitOfWorkIntegrationTests
    {
        private IServiceProvider _serviceProvider;
        private ILiquidUnitOfWork _unitOfWork;
        private ILiquidRepository<TestEntity, int> _sut;

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
            var services = new ServiceCollection();

            services.AddSingleton(Substitute.For<ILogger<LiquidTelemetryInterceptor>>());

            services.AddScoped<ILiquidRepository<TestEntity, int>, InMemoryRepository<TestEntity, int>>();

            services.AddTransient<ILiquidUnitOfWork, LiquidUnitOfWork>();

            _serviceProvider = services.BuildServiceProvider();

            _unitOfWork = _serviceProvider.GetService<ILiquidUnitOfWork>();

            _sut = _unitOfWork.GetRepository<ILiquidRepository<TestEntity, int>, TestEntity, int>();
        }

        [TearDown]
        public void DisposeResources()
        {
            _serviceProvider = null;
            _sut = null;
            _unitOfWork.Dispose();
            _unitOfWork = null;
        }

        [Test]
        public void LiquidUnitOfWorkConstructor_WhenServiceProviderDoesntExists_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new LiquidUnitOfWork(null));
        }

        [Test]
        public void GetRepository_WhenRepositoryDoesntExists_ThrowException()
        {
            Assert.Throws<NullReferenceException>(() => _unitOfWork.GetRepository<ILiquidRepository<AnotherTestEntity, int>, AnotherTestEntity, int>());
        }

        [Test]
        public void GetRepository_WhenRepositoryExists_Success()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<ILiquidUnitOfWork, LiquidUnitOfWork>()
                .AddScoped<ILiquidRepository<TestEntity, int>, InMemoryRepository<TestEntity, int>>()
                .BuildServiceProvider();

            var unitOfWorkWithRepository = new LiquidUnitOfWork(serviceProvider); 

            Assert.IsInstanceOf<ILiquidRepository<TestEntity, int>>(unitOfWorkWithRepository.GetRepository<ILiquidRepository<TestEntity, int>, TestEntity, int>());

            unitOfWorkWithRepository.Dispose();
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

        [Test]
        public void StartTransactionAsync_WhenDataContextDoesntExists_ThrowException()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<ILiquidUnitOfWork, LiquidUnitOfWork>()
                .BuildServiceProvider();

            var unitOfWorkWithoutRepository = new LiquidUnitOfWork(serviceProvider);

            Assert.ThrowsAsync<UnitofWorkTransactionWithoutRepositoryException>(async () => await unitOfWorkWithoutRepository.StartTransactionAsync());

            unitOfWorkWithoutRepository.Dispose();
        }

        [Test]
        public void CommitAsync_WhenNoTransactionIsStarted_ThrowException()
        {
            Assert.ThrowsAsync<UnitOfWorkTransactionNotStartedException>(async () => await _unitOfWork.CommitAsync());
        }

        [Test]
        public void RollbackTransactionAsync_WhenNoTransactionIsStarted_ThrowException()
        {
            Assert.ThrowsAsync<UnitOfWorkTransactionNotStartedException>(async () => await _unitOfWork.RollbackTransactionAsync());
        }
    }
}
