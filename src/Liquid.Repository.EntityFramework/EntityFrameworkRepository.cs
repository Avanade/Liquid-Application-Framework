using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Liquid.Repository.EntityFramework
{
    /// <summary>
    /// Implements the EntityFramework repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    /// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
    /// <seealso cref="ILiquidRepository{TEntity, TIdentifier}" />
    public class EntityFrameworkRepository<TEntity, TIdentifier, TContext> : ILiquidRepository<TEntity, TIdentifier> where TEntity : LiquidEntity<TIdentifier>, new() where TContext : DbContext
    {
        ///<inheritdoc/>
        public IEntityFrameworkDataContext<TContext> EntityDataContext { get; }

        ///<inheritdoc/>
        public ILiquidDataContext DataContext => EntityDataContext;

        private readonly TContext _dbClient;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IQueryable<TEntity> _queryableReadOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepository{TEntity, TIdentifier, TContext}" /> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <exception cref="ArgumentNullException">
        /// telemetryFactory
        /// or
        /// dataContext
        /// </exception>
        public EntityFrameworkRepository(IEntityFrameworkDataContext<TContext> dataContext)
        {
            EntityDataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));

            _dbClient = dataContext.DbClient;
            _dbSet = _dbClient.Set<TEntity>();
            _queryableReadOnly = _dbSet.AsNoTracking();

        }

        ///<inheritdoc/>
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbClient.SaveChangesAsync();
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IEnumerable<TEntity> returnValue = _queryableReadOnly;

            return returnValue;
        }

        ///<inheritdoc/>
        public async Task<TEntity> FindByIdAsync(TIdentifier id)
        {
            var returnValue = _queryableReadOnly.FirstOrDefault(o => o.Id.Equals(id));

            return returnValue;
        }

        ///<inheritdoc/>
        public async Task RemoveByIdAsync(TIdentifier id)
        {
            var obj = await _dbSet.FirstOrDefaultAsync(o => o.Id.Equals(id));

            if (obj == null) return;

            _dbSet.Remove(obj);
            await _dbClient.SaveChangesAsync();
        }

        ///<inheritdoc/>
        public async Task UpdateAsync(TEntity entity)
        {
            _dbClient.Detach<TEntity>(o => o.Id.Equals(entity.Id));
            _dbClient.Update(entity);
            await _dbClient.SaveChangesAsync();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> whereClause)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var result = _queryableReadOnly.Where(whereClause);
            var returnValue = result.AsEnumerable();

            return returnValue;
        }

    }
}
