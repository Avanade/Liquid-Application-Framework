using Liquid.Core.Entities;
using Liquid.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Repository.OData.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ILiquidRepository{TEntity, TIdentifier}"/>.
    /// </summary>
    public static class ILiquidRepositoryExtension
    {
        /// <summary>
        /// Set the token to perform operations.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TIdentifier"></typeparam>
        /// <param name="repository"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ILiquidRepository<TEntity, TIdentifier> SetODataAuthenticationHeader<TEntity, TIdentifier>(this ILiquidRepository<TEntity, TIdentifier> repository, string token) where TEntity : LiquidEntity<TIdentifier>, new()
        {
            var oDataRepository = repository as ODataRepository<TEntity, TIdentifier>;
            
            oDataRepository.SetToken(token);

            return repository;
        }
    }
}
