using Microsoft.EntityFrameworkCore;
using System;

namespace PROJECTNAME.Repository
{
    public class LiquidDbContext : DbContext
    {
        public LiquidDbContext() : base() { }

        public LiquidDbContext(DbContextOptions<LiquidDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
