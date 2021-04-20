using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextExtensions
    {
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
