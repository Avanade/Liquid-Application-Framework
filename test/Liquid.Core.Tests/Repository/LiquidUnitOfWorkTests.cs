using Liquid.Core.Exceptions;
using Liquid.Core.Implementations;
using Liquid.Core.Interfaces;
using Liquid.Core.Tests.Mocks;
using Liquid.Repository.Tests.Mock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Core.Tests.Repository
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

        
        public LiquidUnitOfWorkIntegrationTests()
        {
            var services = new ServiceCollection();

            services.AddSingleton(Substitute.For<ILogger<LiquidTelemetryInterceptor>>());

            services.AddScoped<ILiquidRepository<TestEntity, int>, InMemoryRepository<TestEntity, int>>();

            services.AddTransient<ILiquidUnitOfWork, LiquidUnitOfWork>();

            _serviceProvider = services.BuildServiceProvider();

            _unitOfWork = _serviceProvider.GetService<ILiquidUnitOfWork>();

            _sut = _unitOfWork.GetRepository<ILiquidRepository<TestEntity, int>, TestEntity, int>();
        }

        
        public void DisposeResources()
        {
            _serviceProvider = null;
            _sut = null;
            _unitOfWork.Dispose();
            _unitOfWork = null;
        }

        [Fact]
        public void LiquidUnitOfWorkConstructor_WhenServiceProviderDoesntExists_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new LiquidUnitOfWork(null));
        }

        [Fact]
        public void GetRepository_WhenRepositoryDoesntExists_ThrowException()
        {
            Assert.Throws<NullReferenceException>(() => _unitOfWork.GetRepository<ILiquidRepository<AnotherTestEntity, int>, AnotherTestEntity, int>());
        }

        [Fact]
        public void GetRepository_WhenRepositoryExists_Success()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<ILiquidUnitOfWork, LiquidUnitOfWork>()
                .AddScoped<ILiquidRepository<TestEntity, int>, InMemoryRepository<TestEntity, int>>()
                .BuildServiceProvider();

            var unitOfWorkWithRepository = new LiquidUnitOfWork(serviceProvider);

            Assert.IsAssignableFrom<ILiquidRepository<TestEntity, int>>(unitOfWorkWithRepository.GetRepository<ILiquidRepository<TestEntity, int>, TestEntity, int>());

            unitOfWorkWithRepository.Dispose();
        }

        [Fact]
        public async Task AddAsync_WhenCommitTransaction_ItemAdded()
        {
            await _unitOfWork.StartTransactionAsync();

            await _sut.AddAsync(_entity);

            await _unitOfWork.CommitAsync();

            var result = await _sut.FindByIdAsync(1242);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddAsync_WhenRollbackTransaction_ItemNotInserted()
        {
            await _unitOfWork.StartTransactionAsync();

            await _sut.AddAsync(_entity);

            await _unitOfWork.RollbackTransactionAsync();

            var result = await _sut.FindByIdAsync(1242);

            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveByIdAsync_WhenCommitTransaction_ItemDeleted()
        {
            await _sut.AddAsync(_entity);

            await _unitOfWork.StartTransactionAsync();

            await _sut.RemoveByIdAsync(_entity.Id);

            await _unitOfWork.CommitAsync();

            var result = await _sut.WhereAsync(e => e.Id.Equals(_entity.Id));

            Assert.False(result.Any());
        }

        [Fact]
        public async Task RemoveByIdAsync_WhenRollbackTransaction_ItemNotDeleted()
        {
            await _sut.AddAsync(_entity);

            await _unitOfWork.StartTransactionAsync();

            await _sut.RemoveByIdAsync(_entity.Id);

            await _unitOfWork.RollbackTransactionAsync();

            var result = await _sut.FindByIdAsync(1242);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateAsync_WhenCommitTransaction_ItemNotDeleted()
        {
            await _sut.AddAsync(_entity);

            await _unitOfWork.StartTransactionAsync();

            await _sut.UpdateAsync(_updateEntity);

            await _unitOfWork.CommitAsync();

            var result = await _sut.FindByIdAsync(1242);

            Assert.Equal(_updateEntity.Active, result.Active);
        }

        [Fact]
        public async Task UpdateAsync_WhenRollbackTransaction_ItemNotDeleted()
        {
            await _sut.AddAsync(_entity);

            await _unitOfWork.StartTransactionAsync();

            await _sut.UpdateAsync(_updateEntity);

            await _unitOfWork.RollbackTransactionAsync();

            var result = await _sut.FindByIdAsync(1242);

            Assert.Equal(_entity.Active, result.Active);
        }

        [Fact]
        public void StartTransactionAsync_WhenDataContextDoesntExists_ThrowException()
        {
            var serviceProvider = new ServiceCollection()
                .AddTransient<ILiquidUnitOfWork, LiquidUnitOfWork>()
                .BuildServiceProvider();

            var unitOfWorkWithoutRepository = new LiquidUnitOfWork(serviceProvider);

            Assert.ThrowsAsync<UnitofWorkTransactionWithoutRepositoryException>(async () => await unitOfWorkWithoutRepository.StartTransactionAsync());

            unitOfWorkWithoutRepository.Dispose();
        }

        [Fact]
        public void CommitAsync_WhenNoTransactionIsStarted_ThrowException()
        {
            Assert.ThrowsAsync<UnitOfWorkTransactionNotStartedException>(async () => await _unitOfWork.CommitAsync());
        }

        [Fact]
        public void RollbackTransactionAsync_WhenNoTransactionIsStarted_ThrowException()
        {
            Assert.ThrowsAsync<UnitOfWorkTransactionNotStartedException>(async () => await _unitOfWork.RollbackTransactionAsync());
        }
    }
}
