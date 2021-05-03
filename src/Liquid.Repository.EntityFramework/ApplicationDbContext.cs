using Microsoft.EntityFrameworkCore;

namespace Liquid.Repository.EntityFramework
{
    /// <summary>
    /// Overrides of <see cref="DbContext"/> methods.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        ///<inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
