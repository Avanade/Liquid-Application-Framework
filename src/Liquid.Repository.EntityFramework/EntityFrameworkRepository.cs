using Liquid.Core.Telemetry;
using Liquid.Data.Entities;
using Liquid.Data.EntityFramework.Exceptions;
using Liquid.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Liquid.Data.EntityFramework
{
    public abstract class EntityFrameworkRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier> where TEntity : DataMappingBase<TIdentifier>, new()
    {
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

        public async Task AddAsync(TEntity item)
        {
            var telemetry = _telemetryFactory.GetTelemetry();

            try
            {
                telemetry.EnqueueContext("Database_EntityFramework");
                telemetry.StartTelemetryStopWatchMetric($"AddAsync");

                await _dbSet.AddAsync(item);
                await _dbContext.SaveChangesAsync();

                telemetry.CollectTelemetryStopWatchMetric($"AddAsync");
            }
            catch (Exception e)
            {
                throw new EntityFrameworkException(e);
            }
            finally
            {
                telemetry.DequeueContext("Database_EntityFramework");
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            var telemetry = _telemetryFactory.GetTelemetry();

            try
            {
                telemetry.EnqueueContext("Database_EntityFramework");
                telemetry.StartTelemetryStopWatchMetric($"FindAllAsync");

                var result = _queryableReadOnly;

                telemetry.CollectTelemetryStopWatchMetric($"FindAllAsync");

                return result;
            }
            catch (Exception e)
            {
                throw new EntityFrameworkException(e);
            }
            finally
            {
                telemetry.DequeueContext("Database_EntityFramework");
            }
        }

        public async Task<TEntity> FindByIdAsync(TIdentifier id)
        {
            var telemetry = _telemetryFactory.GetTelemetry();

            try
            {
                telemetry.EnqueueContext("Database_EntityFramework");
                telemetry.StartTelemetryStopWatchMetric($"FindByIdAsync");

                var result = await _queryableReadOnly.FirstOrDefaultAsync(o => o.Id.Equals(id));

                telemetry.CollectTelemetryStopWatchMetric($"FindByIdAsync");

                return result;
            }
            catch (Exception e)
            {
                throw new EntityFrameworkException(e);
            }
            finally
            {
                telemetry.DequeueContext("Database_EntityFramework");
            }
        }

        public async Task RemoveAsync(TEntity item)
        {

            var telemetry = _telemetryFactory.GetTelemetry();

            try
            {
                telemetry.EnqueueContext("Database_EntityFramework");
                telemetry.StartTelemetryStopWatchMetric($"RemoveAsync");

                var obj = await _dbSet.FirstOrDefaultAsync(o => o.Id.Equals(item.Id));

                if (obj == null) return;

                _dbSet.Remove(obj);
                await _dbContext.SaveChangesAsync();

                telemetry.CollectTelemetryStopWatchMetric($"RemoveAsync");
            }
            catch (Exception e)
            {
                throw new EntityFrameworkException(e);
            }
            finally
            {
                telemetry.DequeueContext("Database_EntityFramework");
            }
        }

        public async Task UpdateAsync(TEntity item)
        {
            var telemetry = _telemetryFactory.GetTelemetry();

            try
            {
                telemetry.EnqueueContext("Database_EntityFramework");
                telemetry.StartTelemetryStopWatchMetric($"UpdateAsync");

                _dbContext.Detach<TEntity>(o => o.Id.Equals(item.Id));
                _dbContext.Update(item);
                await _dbContext.SaveChangesAsync();

                telemetry.CollectTelemetryStopWatchMetric($"UpdateAsync");
            }
            catch (Exception e)
            {
                throw new EntityFrameworkException(e);
            }
            finally
            {
                telemetry.DequeueContext("Database_EntityFramework");
            }
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> whereClause)
        {
            var telemetry = _telemetryFactory.GetTelemetry();

            try
            {
                telemetry.EnqueueContext("Database_EntityFramework");
                telemetry.StartTelemetryStopWatchMetric($"UpdateAsync");

               var result = _queryableReadOnly.Where(whereClause);

                telemetry.CollectTelemetryStopWatchMetric($"UpdateAsync");

                return result;
            }
            catch (Exception e)
            {
                throw new EntityFrameworkException(e);
            }
            finally
            {
                telemetry.DequeueContext("Database_EntityFramework");
            }
        }

        public async Task<IEnumerable<TEntity>> WhereByPageAsync(Expression<Func<TEntity, bool>> whereClause, int page, int itemsPerPage, Expression<Func<TEntity, object>> orderByClause = null, bool sortAscending = true)
        {
            var telemetry = _telemetryFactory.GetTelemetry();

            try
            {
                telemetry.EnqueueContext("Database_EntityFramework");
                telemetry.StartTelemetryStopWatchMetric($"UpdateAsync");

                var query = _queryableReadOnly.Where(whereClause);

                if (orderByClause != null)
                    if (sortAscending) query = query.OrderBy(orderByClause);
                    else query = query.OrderByDescending(orderByClause);

                var result =  query.Skip(page * itemsPerPage).Take(itemsPerPage);

                telemetry.CollectTelemetryStopWatchMetric($"UpdateAsync");

                return result;
            }
            catch (Exception e)
            {
                throw new EntityFrameworkException(e);
            }
            finally
            {
                telemetry.DequeueContext("Database_EntityFramework");
            }
        }
    }
}
