using Liquid.Core.Telemetry;
using Liquid.Repository.EntityFramework.Extensions;
using Liquid.Repository.EntityFramework.Tests.Entities;
using Liquid.Repository.EntityFramework.Tests.Repositories;
using Liquid.Repository.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Liquid.Repository.EntityFramework.Tests
{
    [TestFixture()]
    public class EntityFrameworkRepositoryTest
    {
        private IServiceProvider _serviceProvider;

        [SetUp]
        public async Task EstablishContext()
        {
            var services = new ServiceCollection();
            var databaseName = $"TEMP_{Guid.NewGuid()}";

            services.AddDbContext<MockDbContext>(options => options.UseInMemoryDatabase(databaseName: databaseName));

            services.AddTransient((s) => Substitute.For<ILightTelemetryFactory>());

            services.AddEntityFramework<MockDbContext>(GetType().Assembly);

            _serviceProvider = services.BuildServiceProvider();

            await SeedDataAsync(_serviceProvider);
        }

        [Category("AddAsync")]
        [Test]
        public async Task Verify_insert()
        {
            //Arrange
            IMockRepository mockRepository = GenerateMockRepository();
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

            IMockRepository mockRepository = GenerateMockRepository(dbSet);
            var entity = new MockEntity { MockTitle = "TITLE", Active = true, CreatedDate = DateTime.Now };

            //Act
            var task = mockRepository.AddAsync(entity);

            //Assert
            Assert.ThrowsAsync<DatabaseContextException>(() => task);
        }

        [Category("FindByIdAsync")]
        [Test]
        public async Task Verify_find_by_id()
        {
            //Arrange
            IMockRepository mockRepository = GenerateMockRepository();
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
            IMockRepository mockRepository = GenerateMockRepository(dbSet);
            var mockId = 1;

            //Act
            var task = mockRepository.FindByIdAsync(mockId);

            //Assert
            Assert.ThrowsAsync<DatabaseContextException>(() => task);
        }

        [Category("WhereAsync")]
        [Test]
        public async Task Verify_where()
        {
            //Arrange
            IMockRepository mockRepository = GenerateMockRepository();
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
            IMockRepository mockRepository = GenerateMockRepository(dbSet);
            string mockTitle = "TITLE_002";

            //Act
            var task = mockRepository.WhereAsync(o => o.MockTitle.Equals(mockTitle));

            //Assert
            Assert.ThrowsAsync<DatabaseContextException>(() => task);
        }

        [Category("GetAllAsync")]
        [Test]
        public async Task Verify_find_all()
        {
            //Arrange
            IMockRepository mockRepository = GenerateMockRepository();

            //Act
            var result = await mockRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(100, result.Count());
        }

        [Category("FindAllAsync")]
        [Test]
        public void Verify_find_all_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            var telemetryFactory = Substitute.For<ILightTelemetryFactory>();
            var telemetryClient = Substitute.For<ILightTelemetry>();
            telemetryClient.When(o => o.AddContext(Arg.Any<string>())).Do((call) => throw new Exception());
            telemetryFactory.GetTelemetry().Returns(telemetryClient);
            IMockRepository mockRepository = GenerateMockRepository(dbSet, telemetryFactory);

            //Act
            var task = mockRepository.GetAllAsync();

            //Assert
            Assert.ThrowsAsync<DatabaseContextException>(() => task);
        }

        [Category("RemoveAsync")]
        [Test]
        public async Task Verify_delete()
        {
            //Arrange
            IMockRepository mockRepository = GenerateMockRepository();
            var mockId = 1;

            //Act
            var entity = await mockRepository.FindByIdAsync(mockId);
            await mockRepository.RemoveAsync(entity);
            var anotherEntity = await mockRepository.FindByIdAsync(mockId);

            //Assert
            Assert.IsNull(anotherEntity);
        }
        [Category("RemoveAsync")]
        [Test]
        public async Task Verify_delete_invalid()
        {
            //Arrange
            IMockRepository mockRepository = GenerateMockRepository();
            var mockId = 101;

            //Act
            var entity = await mockRepository.FindByIdAsync(mockId);
            await mockRepository.RemoveAsync(new MockEntity() { MockId = mockId });
            var anotherEntity = await mockRepository.FindByIdAsync(mockId);

            //Assert
            Assert.IsNull(entity);
            Assert.IsNull(anotherEntity);
        }

        [Category("RemoveAsync")]
        [Test]
        public void Verify_delete_Except()
        {
            //Arrange
            var dbSet = Substitute.For<DbSet<MockEntity>, IQueryable<MockEntity>>();
            IMockRepository mockRepository = GenerateMockRepository(dbSet);
            var mockId = 1;

            //Act
            var task = mockRepository.RemoveAsync(new MockEntity() { MockId = mockId });

            //Assert
            Assert.ThrowsAsync<DatabaseContextException>(() => task);
        }

        [Category("UpdateAsync")]
        [Test]
        public async Task Verify_updates()
        {
            //Arrange
            IMockRepository mockRepository = GenerateMockRepository();
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
            IMockRepository mockRepository = GenerateMockRepository(dbSet);
            var mockId = 1;

            //Act
            var task = mockRepository.UpdateAsync(new MockEntity() { MockId = mockId });

            //Assert
            Assert.ThrowsAsync<DatabaseContextException>(() => task);
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

        private IMockRepository GenerateMockRepository()
        {
            return _serviceProvider.GetService<IMockRepository>();
        }

        private IMockRepository GenerateMockRepository(DbSet<MockEntity> dbSet, ILightTelemetryFactory telemetryFactory = null)
        {
            var dbContext = Substitute.For<MockDbContext>();
            dbContext.Set<MockEntity>().Returns(dbSet);

            var dataContext = Substitute.For<IEntityFrameworkDataContext<MockDbContext>>();

            dataContext.DbClient.Returns(dbContext);

            telemetryFactory = telemetryFactory ?? _serviceProvider.GetService<ILightTelemetryFactory>();

            return new MockRepository(dataContext, telemetryFactory);
        }
    }
}