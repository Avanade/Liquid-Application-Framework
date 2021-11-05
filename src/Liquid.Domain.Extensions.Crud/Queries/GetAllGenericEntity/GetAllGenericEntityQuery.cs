using Liquid.Repository;
using MediatR;

namespace Liquid.Domain.Extensions.Crud.Queries.GetAllGenericEntity
{
    /// <summary>
    /// Query to Get All Entities
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public class GetAllGenericEntityQuery<TEntity, TIdentifier> : IRequest<GetAllGenericEntityQueryResponse<TEntity>> where TEntity : LiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Initialize an instance of <see cref="GetAllGenericEntityQuery{TEntity, TIdentifier}"/>
        /// </summary>
        public GetAllGenericEntityQuery() { }
    }
}