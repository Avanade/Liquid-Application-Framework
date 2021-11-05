using Liquid.Repository;
using MediatR;

namespace Liquid.Domain.Extensions.Crud.Queries.FindByIdGenericEntity
{
    /// <summary>
    /// Query to Find Entity to Id
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public class FindByIdGenericEntityQuery<TEntity, TIdentifier> : IRequest<FindByIdGenericEntityQueryResponse<TEntity>> where TEntity : LiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Id of Entity to Find
        /// </summary>
        public TIdentifier Id { get; set; }

        /// <summary>
        /// Initialize an instance of <see cref="FindByIdGenericEntityQuery{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="id">Id to Find</param>
        public FindByIdGenericEntityQuery(TIdentifier id)
        {
            Id = id;
        }
    }
}
