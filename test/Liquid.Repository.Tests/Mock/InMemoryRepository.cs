using Liquid.Repository.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Repository.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class InMemoryRepository<TEntity, TIdentifier> : ILiquidRepository<TEntity, TIdentifier>, ILiquidDataContext where TEntity : LiquidEntity<TIdentifier>, new()
    {
        public ILiquidDataContext DataContext { get { return this; } }

        public string Id { get; }

        private Dictionary<TIdentifier, TEntity> _inMemoryRepository;
        private Dictionary<TIdentifier, TEntity> _inMemoryTempTransactionRepository;

        private bool disposedValue;

        public InMemoryRepository()
        {
            _inMemoryRepository = new Dictionary<TIdentifier, TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await Task.FromResult<bool>(_inMemoryRepository.TryAdd(entity.Id, entity));
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            var entities = await Task.FromResult<IEnumerable<TEntity>>(_inMemoryRepository.Values);
            return entities;
        }

        public async Task<TEntity> FindByIdAsync(TIdentifier id)
        {
            var entity = await Task.FromResult<TEntity>(_inMemoryRepository.GetValueOrDefault(id));
            return entity;
        }

        public async Task RemoveByIdAsync(TIdentifier id)
        {
            await Task.FromResult<bool>(_inMemoryRepository.Remove(id));
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await Task.FromResult<bool>(_inMemoryRepository.Remove(entity.Id));
            await Task.FromResult<bool>(_inMemoryRepository.TryAdd(entity.Id, entity));
        }

        public async Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> whereClause)
        {
            var selectableCollection = _inMemoryRepository.Values.AsQueryable<TEntity>();
            var entities = await Task.FromResult<IEnumerable<TEntity>>(selectableCollection.Where(whereClause));
            return entities;
        }

        public async Task StartTransactionAsync()
        {
            _inMemoryTempTransactionRepository = await Task.FromResult(cloneRepository(_inMemoryRepository));
        }

        public async Task CommitAsync()
        {
            await Task.Run(() => { _inMemoryTempTransactionRepository = null; });
        }

        public async Task RollbackTransactionAsync()
        {
            _inMemoryRepository = await Task.FromResult(cloneRepository(_inMemoryTempTransactionRepository));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _inMemoryRepository = null;
                    _inMemoryTempTransactionRepository = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private Dictionary<TIdentifier, TEntity> cloneRepository(Dictionary<TIdentifier, TEntity> repoToClone)
        {
            var clone = new Dictionary<TIdentifier, TEntity>();
            foreach(var key in repoToClone.Keys)
            {
                clone.Add(key, repoToClone[key]);
            }

            return clone;
        }
    }
}
