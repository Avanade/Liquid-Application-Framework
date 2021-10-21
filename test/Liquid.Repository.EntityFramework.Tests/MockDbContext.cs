using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Liquid.Repository.EntityFramework.Tests
{
    [ExcludeFromCodeCoverage]
    public class MockDbContext : DbContext
    {
        public MockDbContext() : base() { }

        public MockDbContext(DbContextOptions<MockDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
