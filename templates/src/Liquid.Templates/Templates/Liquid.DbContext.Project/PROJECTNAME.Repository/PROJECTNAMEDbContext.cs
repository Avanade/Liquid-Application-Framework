using Microsoft.EntityFrameworkCore;
using System;

namespace PROJECTNAME.Repository
{
    public class PROJECTNAMEDbContext : DbContext
    {
        public PROJECTNAMEDbContext() : base() { }

        public PROJECTNAMEDbContext(DbContextOptions<PROJECTNAMEDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
