using Liquid.Core.Interfaces;
using Liquid.Repository.EntityFramework.Extensions;
using Liquid.Repository.EntityFramework.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Liquid.Repository.EntityFramework.Tests
{
    [ExcludeFromCodeCoverage]
    public class EntityFrameworkRepositoryTest
    {
        private IServiceProvider _serviceProvider;


        public EntityFrameworkRepositoryTest()
        {
            var services = new ServiceCollection();
            var databaseName = $"TEMP_{Guid.NewGuid()}";

            services.AddLiquidEntityFramework<MockDbContext, MockEntity, int>(options => options.UseInMemoryDatabase(databaseName: databaseName));

            _serviceProvider = services.BuildServiceProvider();

            SeedDataAsync(_serviceProvider);
        }

        private EntityFrameworkRepository<MockEntity, int, MockDbContext> GenerateMockRepository()
        {
            return _serviceProvider.GetService<EntityFrameworkRepository<MockEntity, int, MockDbContext>>();
        }

        private EntityFrameworkRepository<MockEntity, int, MockDbContext> GenerateMockRepository(DbSet<MockEntity> dbSet)
        {
            var dbContext = Substitute.For<MockDbContext>();
            dbContext.Set<MockEntity>().Returns(dbSet);

            var dataContext = Substitute.For<IEntityFrameworkDataContext<MockDbContext>>();

            dataContext.DbClient.Returns(dbContext);

            return new EntityFrameworkRepository<MockEntity, int, MockDbContext>(dataContext);
        }
                
        [Fact]
        public async Task Verify_insert()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();
            var entity = new MockEntity { MockTitle = "TITLE", Active = true, CreatedDate = DateTime.Now };

            //Act
            await mockRepository.AddAsync(entity);

            //Assert
            Assert.NotNull(entity);
            Assert.NotEqual(default, entity.MockId);
        }

        [Fact]
        public async Task Verify_insert_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            dbSet.When(o => o.AddAsync(Arg.Any<MockEntity>())).Do((call) => throw new Exception());

            var mockRepository = GenerateMockRepository(dbSet);
            var entity = new MockEntity { MockTitle = "TITLE", Active = true, CreatedDate = DateTime.Now };

            //Act
            var task = mockRepository.AddAsync(entity);

            //Assert
            await Assert.ThrowsAsync<Exception>(() => task);
        }

        [Fact]
        public async Task Verify_find_by_id()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();
            var mockId = 1;

            //Act
            var entity = await mockRepository.FindByIdAsync(mockId);

            //Assert
            Assert.NotNull(entity);
            Assert.Equal(mockId, entity.MockId);
        }

        [Fact]
        public async Task Verify_find_by_id_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            var mockRepository = GenerateMockRepository(dbSet);
            var mockId = 1;

            //Act
            var task = mockRepository.FindByIdAsync(mockId);

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => task);
        }

        [Fact]
        public async Task Verify_where()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();
            string mockTitle = "TITLE_002";

            //Act
            var result = await mockRepository.WhereAsync(o => o.MockTitle.Equals(mockTitle));

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.All(o => o.MockTitle.Equals(mockTitle)));
        }

        [Fact]
        public async Task Verify_where_Except()
        {
            //Arrange
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            var mockRepository = GenerateMockRepository(dbSet);
            string mockTitle = "TITLE_002";

            //Act
            var task = mockRepository.WhereAsync(o => o.MockTitle.Equals(mockTitle));

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => task);
        }

        [Fact]
        public async Task Verify_find_all()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();

            //Act
            var result = await mockRepository.FindAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(100, result.Count());
        }

        [Fact]
        public async Task Verify_find_all_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();

            var mockRepository = GenerateMockRepository(dbSet);

            //Act
            var result = await mockRepository.FindAllAsync();

            //Assert
            Assert.Empty(result);
        }             

        [Fact]
        public async Task Verify_delete()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();
            var mockId = 1;

            //Act
            await mockRepository.RemoveByIdAsync(mockId);
            var anotherEntity = await mockRepository.FindByIdAsync(mockId);

            //Assert
            Assert.Null(anotherEntity);
        }

        [Fact]
        public async Task Verify_delete_invalid()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();
            var mockId = 101;

            //Act
            await mockRepository.RemoveByIdAsync(mockId);
            var anotherEntity = await mockRepository.FindByIdAsync(mockId);

            //Assert
            Assert.Null(anotherEntity);
        }

        [Fact]
        public async Task Verify_delete_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            var mockRepository = GenerateMockRepository(dbSet);
            var mockId = 1;

            //Act
            var task = mockRepository.RemoveByIdAsync(mockId);

            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => task);
        }

        [Fact]
        public async Task Verify_updates()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();
            var mockId = 1;

            //Act
            var entity = await mockRepository.FindByIdAsync(mockId);
            entity.MockTitle = $"TITLE_001_UPDATED";
            await mockRepository.UpdateAsync(entity);
            var anotherEntity = await mockRepository.FindByIdAsync(mockId);

            //Assert
            Assert.NotNull(anotherEntity);
            Assert.Equal("TITLE_001_UPDATED", anotherEntity.MockTitle);
        }

        [Fact]
        public async Task Verify_updates_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            var mockRepository = GenerateMockRepository(dbSet);
            var mockId = 1;

            //Act
            var task = mockRepository.UpdateAsync(new MockEntity() { MockId = mockId });

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => task);
        }

        private void SeedDataAsync(IServiceProvider serviceProvider)
        {
            MockDbContext dbContext = serviceProvider.GetService<MockDbContext>();

            for (int i = 1; i <= 100; i++)
            {
                dbContext.AddAsync(new MockEntity() { MockId = i, MockTitle = $"TITLE_{i:000}", Active = true, CreatedDate = DateTime.Now })
                    .GetAwaiter().GetResult();
            }
            dbContext.SaveChangesAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void EntityFrameworkRepository_WhenCreatedWithoutDataContext_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new EntityFrameworkRepository<MockEntity, int, MockDbContext>(null));
        }

        [Fact]
        public void EntityFrameworkRepository_WhenCreated_DataContextIsValid()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            var mockRepository = GenerateMockRepository(dbSet);

            //Act
            var dataContext = mockRepository.DataContext;
            var entityFrameworkDataContext = mockRepository.EntityDataContext;

            //Assert
            Assert.NotNull(dataContext);
            Assert.IsAssignableFrom<ILiquidDataContext>(dataContext);

            Assert.NotNull(entityFrameworkDataContext);
            Assert.IsAssignableFrom<IEntityFrameworkDataContext<MockDbContext>>(entityFrameworkDataContext);
        }
    }
}