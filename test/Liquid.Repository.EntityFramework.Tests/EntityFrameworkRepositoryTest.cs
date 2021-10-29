using Liquid.Repository.EntityFramework.Extensions;
using Liquid.Repository.EntityFramework.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Liquid.Repository.EntityFramework.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture()]
    public class EntityFrameworkRepositoryTest
    {
        private IServiceProvider _serviceProvider;

        [SetUp]
        public async Task EstablishContext()
        {
            var services = new ServiceCollection();
            var databaseName = $"TEMP_{Guid.NewGuid()}";

            services.AddLiquidEntityFramework<MockDbContext, MockEntity, int>(options => options.UseInMemoryDatabase(databaseName: databaseName));

            _serviceProvider = services.BuildServiceProvider();

            await SeedDataAsync(_serviceProvider);
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

        [Category("AddAsync")]
        [Test]
        public async Task Verify_insert()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();
            var entity = new MockEntity { MockTitle = "TITLE", Active = true, CreatedDate = DateTime.Now };

            //Act
            await mockRepository.AddAsync(entity);

            //Assert
            Assert.NotNull(entity);
            Assert.AreNotEqual(default, entity.MockId);
        }

        [Category("AddAsync")]
        [Test]
        public void Verify_insert_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            dbSet.When(o => o.AddAsync(Arg.Any<MockEntity>())).Do((call) => throw new Exception());

            var mockRepository = GenerateMockRepository(dbSet);
            var entity = new MockEntity { MockTitle = "TITLE", Active = true, CreatedDate = DateTime.Now };

            //Act
            var task = mockRepository.AddAsync(entity);

            //Assert
            Assert.ThrowsAsync<Exception>(() => task);
        }

        [Category("FindByIdAsync")]
        [Test]
        public async Task Verify_find_by_id()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();
            var mockId = 1;

            //Act
            var entity = await mockRepository.FindByIdAsync(mockId);

            //Assert
            Assert.NotNull(entity);
            Assert.AreEqual(mockId, entity.MockId);
        }
        [Category("FindByIdAsync")]
        [Test]
        public void Verify_find_by_id_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            var mockRepository = GenerateMockRepository(dbSet);
            var mockId = 1;

            //Act
            var task = mockRepository.FindByIdAsync(mockId);

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => task);
        }

        [Category("WhereAsync")]
        [Test]
        public async Task Verify_where()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();
            string mockTitle = "TITLE_002";

            //Act
            var result = await mockRepository.WhereAsync(o => o.MockTitle.Equals(mockTitle));

            //Assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.All(o => o.MockTitle.Equals(mockTitle)));
        }

        [Category("WhereAsync")]
        [Test]
        public void Verify_where_Except()
        {
            //Arrange
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            var mockRepository = GenerateMockRepository(dbSet);
            string mockTitle = "TITLE_002";

            //Act
            var task = mockRepository.WhereAsync(o => o.MockTitle.Equals(mockTitle));

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => task);
        }

        [Category("GetAllAsync")]
        [Test]
        public async Task Verify_find_all()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();

            //Act
            var result = await mockRepository.FindAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(100, result.Count());
        }

        [Category("FindAllAsync")]
        [Test]
        public async Task Verify_find_all_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();

            var mockRepository = GenerateMockRepository(dbSet);

            //Act
            var result = await mockRepository.FindAllAsync();

            //Assert
            Assert.IsEmpty(result);
        }             

        [Category("RemoveByIdAsync")]
        [Test]
        public async Task Verify_delete()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();
            var mockId = 1;

            //Act
            await mockRepository.RemoveByIdAsync(mockId);
            var anotherEntity = await mockRepository.FindByIdAsync(mockId);

            //Assert
            Assert.IsNull(anotherEntity);
        }

        [Category("RemoveByIdAsync")]
        [Test]
        public async Task Verify_delete_invalid()
        {
            //Arrange
            var mockRepository = GenerateMockRepository();
            var mockId = 101;

            //Act
            await mockRepository.RemoveByIdAsync(mockId);
            var anotherEntity = await mockRepository.FindByIdAsync(mockId);

            //Assert
            Assert.IsNull(anotherEntity);
        }

        [Category("RemoveByIdAsync")]
        [Test]
        public void Verify_delete_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            var mockRepository = GenerateMockRepository(dbSet);
            var mockId = 1;

            //Act
            var task = mockRepository.RemoveByIdAsync(mockId);

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => task);
        }

        [Category("UpdateAsync")]
        [Test]
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
            Assert.AreEqual("TITLE_001_UPDATED", anotherEntity.MockTitle);
        }

        [Category("UpdateAsync")]
        [Test]
        public void Verify_updates_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            var mockRepository = GenerateMockRepository(dbSet);
            var mockId = 1;

            //Act
            var task = mockRepository.UpdateAsync(new MockEntity() { MockId = mockId });

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => task);
        }

        private async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            MockDbContext dbContext = serviceProvider.GetService<MockDbContext>();

            for (int i = 1; i <= 100; i++)
            {
                await dbContext.AddAsync(new MockEntity() { MockId = i, MockTitle = $"TITLE_{i:000}", Active = true, CreatedDate = DateTime.Now });
            }
            await dbContext.SaveChangesAsync();
        }

        [Category("ctor")]
        [Test]
        public void EntityFrameworkRepository_WhenCreatedWithoutDataContext_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new EntityFrameworkRepository<MockEntity, int, MockDbContext>(null));
        }

        [Category("Members")]
        [Test]
        public void EntityFrameworkRepository_WhenCreated_DataContextIsValid()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            var mockRepository = GenerateMockRepository(dbSet);

            //Act
            var dataContext = mockRepository.DataContext;
            var entityFrameworkDataContext = mockRepository.EntityDataContext;

            //Assert
            Assert.IsNotNull(dataContext);
            Assert.IsInstanceOf<ILiquidDataContext>(dataContext);

            Assert.IsNotNull(entityFrameworkDataContext);
            Assert.IsInstanceOf<IEntityFrameworkDataContext<MockDbContext>>(entityFrameworkDataContext);
        }
    }
}