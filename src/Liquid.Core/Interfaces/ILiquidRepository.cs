﻿using Liquid.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Liquid.Core.Interfaces
{
    /// <summary>
    /// This interface represents a single repository for the specific <typeparamref name="TEntity"/> entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    public interface ILiquidRepository<TEntity, TIdentifier> where TEntity : LiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Gets the data context associated to repository.
        /// </summary>
        /// <value>
        /// The data context.
        /// </value>
        ILiquidDataContext DataContext { get; }

        /// <summary>
        /// Adds the specified entity item in repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Removes the specified entity item from repository by informed Id.
        /// </summary>
        /// <param name="id">Identifier of entity that should be removed.</param>
        /// <returns></returns>
        Task RemoveByIdAsync(TIdentifier id);

        /// <summary>
        /// Updates the specified entity item in repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Finds the entity by identifier.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <returns>The entity</returns>
        Task<TEntity> FindByIdAsync(TIdentifier id);

        /// <summary>
        /// Finds the specified entity by the search predicate.
        /// </summary>
        /// <param name="whereClause">The search predicate.</param>
        /// <returns>
        /// List of entities.
        /// </returns>
        Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> whereClause);

        /// <summary>
        /// Retrieve all entities.
        /// </summary>
        /// <returns>List of all entities.</returns>
        Task<IEnumerable<TEntity>> FindAllAsync();
    }
}
