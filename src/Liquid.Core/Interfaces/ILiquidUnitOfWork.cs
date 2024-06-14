﻿using Liquid.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Liquid.Core.Interfaces
{
    /// <summary>
    /// Interface responsible for Managing the repositories transactions.
    /// </summary>
    public interface ILiquidUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository from Service Provider and adds to UnitOfWork.
        /// </summary>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
        /// <returns></returns>
        TRepository GetRepository<TRepository, TEntity, TIdentifier>()
            where TRepository : ILiquidRepository<TEntity, TIdentifier>
            where TEntity : LiquidEntity<TIdentifier>;

        /// <summary>
        /// Starts the transaction of all data contexts in repositories inside UnitOfWork.
        /// </summary>
        /// <returns></returns>
        Task StartTransactionAsync();

        /// <summary>
        /// Commits all commands added to the database context.
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        /// <summary>
        /// Rollbacks the transactions.
        /// </summary>
        /// <returns></returns>
        Task RollbackTransactionAsync();
    }
}
