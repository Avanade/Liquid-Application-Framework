using Liquid.Core.Telemetry;
using Liquid.Repository;
using Liquid.Repository.EntityFramework;
using Liquid.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Liquid.Data.EntityFramework
{
    /// <summary>
    /// Implements the EntityFramework repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    /// <seealso cref="Liquid.Repository.ILightRepository{TEntity, TIdentifier}" />
    public abstract class EntityFrameworkRepository<TEntity, TIdentifier> : ILightRepository<TEntity, TIdentifier> where TEntity : RepositoryEntity<TIdentifier>, new()
    {
        ///<inheritdoc/>
        public IEntityFrameworkDataContext EntityDataContext { get; }

        ///<inheritdoc/>
        public ILightDataContext DataContext => EntityDataContext;

        private readonly DbContext _dbClient;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IQueryable<TEntity> _queryableReadOnly;
        private readonly ILightTelemetryFactory _telemetryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepository{TEntity, TIdentifier}" /> class.
        /// </summary>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="dataContext">The data context.</param>
        /// <exception cref="System.ArgumentNullException">
        /// telemetryFactory
        /// or
        /// dataContext
        /// </exception>
        protected EntityFrameworkRepository(IEntityFrameworkDataContext dataContext, ILightTelemetryFactory telemetryFactory)
        {
            EntityDataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _telemetryFactory = telemetryFactory ?? throw new ArgumentNullException(nameof(telemetryFactory));

            _dbClient = dataContext.DbClient;
            _dbSet = _dbClient.Set<TEntity>();
            _queryableReadOnly = _dbSet.AsNoTracking();
            
        }

        ///<inheritdoc/>
        public async Task AddAsync(TEntity entity)
        {
            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_AddAsync", async () =>
            {
                await _dbSet.AddAsync(entity);
                await _dbClient.SaveChangesAsync();
            });
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IEnumerable<TEntity> returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_GetAllAsync", () => {
                returnValue = _queryableReadOnly;
                return Task.CompletedTask;
            });

            return returnValue;
        }

        ///<inheritdoc/>
        public async Task<TEntity> FindByIdAsync(TIdentifier id)
        {
            TEntity returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_FindByIdAsync", () => {
                returnValue = _queryableReadOnly.FirstOrDefault(o => o.Id.Equals(id));
                return Task.CompletedTask;
            });

            return returnValue;
        }

        ///<inheritdoc/>
        public async Task RemoveAsync(TEntity entity)
        {
            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_RemoveAsync", async () =>
            {
                var obj = await _dbSet.FirstOrDefaultAsync(o => o.Id.Equals(entity.Id));

                if (obj == null) return;
                    
                _dbSet.Remove(obj);
                await _dbClient.SaveChangesAsync();
            });
        }

        ///<inheritdoc/>
        public async Task UpdateAsync(TEntity entity)
        {
            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_UpdateAsync", async () =>
            {
                _dbClient.Detach<TEntity>(o => o.Id.Equals(entity.Id));
                _dbClient.Update(entity);
                await _dbClient.SaveChangesAsync();
            });
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> whereClause)
        {
            IEnumerable<TEntity> returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_WhereAsync", () => {
                var result = _queryableReadOnly.Where(whereClause);
                returnValue = result.AsEnumerable();
                return Task.CompletedTask;
            });

            return returnValue;
        }

    }
}
