using Liquid.Core.Entities;
using Liquid.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Liquid.Repository.EntityFramework.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ILiquidRepository{TEntity, TIdentifier}"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ILiquidRepositoryExtensions
    {
        /// <summary>
        /// Gets the list of entities that matches the where clause,
        /// including the related entities. 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TIdentifier"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="repository"></param>
        /// <param name="whereClause"> The where clause.</param>
        /// <param name="include"> The include clause.</param>
        /// <returns></returns>
        public static ILiquidRepository<TEntity, TIdentifier> WhereInclude<TEntity, TIdentifier, TContext>(this ILiquidRepository<TEntity, TIdentifier> repository
            , Expression<Func<TEntity, bool>> whereClause, Expression<Func<TEntity, object>> include = null) 
            where TEntity : LiquidEntity<TIdentifier>, new() where TContext : DbContext
        {
            var EfRepository = repository as EntityFrameworkRepository<TEntity, TIdentifier,TContext>;

            EfRepository.WhereInclude(whereClause, include);

            return repository;
        }

        /// <summary>
        /// Gets the list of entities that matches the where clause,
        /// including the related entities.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TIdentifier"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="repository"></param>
        /// <param name="whereClause">where clause.</param>
        /// <param name="includes">Entities to include.</param>
        /// <returns></returns>
        public static ILiquidRepository<TEntity, TIdentifier> WhereInclude<TEntity, TIdentifier, TContext>(this ILiquidRepository<TEntity, TIdentifier> repository
                       , Expression<Func<TEntity, bool>> whereClause, string[] includes)
            where TEntity : LiquidEntity<TIdentifier>, new() where TContext : DbContext
        {
            var EfRepository = repository as EntityFrameworkRepository<TEntity, TIdentifier, TContext>;

            EfRepository.WhereInclude(whereClause, includes);

            return repository;
        }
    }
}
