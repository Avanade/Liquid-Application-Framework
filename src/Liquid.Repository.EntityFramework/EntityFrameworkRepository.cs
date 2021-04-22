using Liquid.Core.Telemetry;
using Liquid.Repository;
using Liquid.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Liquid.Data.EntityFramework
{
    public abstract class EntityFrameworkRepository<TEntity, TIdentifier> : ILightRepository<TEntity, TIdentifier> where TEntity : RepositoryEntity<TIdentifier>, new()
    {
        public IEntityFrameworkDataContext EntityFrameworkDataContext { get; }
        public ILightDataContext DataContext => EntityFrameworkDataContext;

        private readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IQueryable<TEntity> _queryableReadOnly;
        protected readonly ILightTelemetryFactory _telemetryFactory;

        protected EntityFrameworkRepository(DbContext dbContext, ILightTelemetryFactory telemetryFactory)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
            _queryableReadOnly = _dbSet.AsNoTracking();
            _telemetryFactory = telemetryFactory;
        }

        ///<inheritdoc/>
        public async Task AddAsync(TEntity item)
        {
            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_AddAsync", async () =>
            {
                await _dbSet.AddAsync(item);
                await _dbContext.SaveChangesAsync();
            });

        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            IEnumerable<TEntity> returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_GetAllAsync", async () =>
            {
                returnValue = _queryableReadOnly;
            });

            return returnValue;
        }

        ///<inheritdoc/>
        public async Task<TEntity> FindByIdAsync(TIdentifier id)
        {
            TEntity returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_FindByIdAsync", async () =>
            {   
                returnValue = _queryableReadOnly.FirstOrDefault(o => o.Id.Equals(id));
            });

            return returnValue;
        }

        ///<inheritdoc/>
        public async Task RemoveAsync(TEntity item)
        {
            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_RemoveAsync", async () =>
            {
                var obj = await _dbSet.FirstOrDefaultAsync(o => o.Id.Equals(item.Id));

                if (obj == null) return;
                    
                _dbSet.Remove(obj);
                await _dbContext.SaveChangesAsync();
            });
        }

        ///<inheritdoc/>
        public async Task UpdateAsync(TEntity item)
        {
            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_UpdateAsync", async () =>
            {
                _dbContext.Detach<TEntity>(o => o.Id.Equals(item.Id));
                _dbContext.Update(item);
                await _dbContext.SaveChangesAsync();
            });
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> whereClause)
        {
            IEnumerable<TEntity> returnValue = null;

            await _telemetryFactory.ExecuteActionAsync("EntityFrameworkRepository_WhereAsync", async () =>
            {
                var result = _queryableReadOnly.Where(whereClause);
                returnValue = result.AsEnumerable();
            });

            return returnValue;
        }

    }
}
