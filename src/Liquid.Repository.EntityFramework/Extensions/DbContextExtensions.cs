using System;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Entity Framework <see cref="DbContext"/> extension methods.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Untrack entities onto <see cref="DbContext"/>.
        /// </summary>
        /// <typeparam name="TEntity">type of entity.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="predicate">Entities filter.</param>
        public static void Detach<TEntity>(this DbContext dbContext, Func<TEntity, bool> predicate) where TEntity : class
        {
            var entities = dbContext.Set<TEntity>().Local.Where(predicate);
            foreach (var entity in entities)
            {
                dbContext.Entry(entity).State = EntityState.Detached;
            }
        }
    }
}
