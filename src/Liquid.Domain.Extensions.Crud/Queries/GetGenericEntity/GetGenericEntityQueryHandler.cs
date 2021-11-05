using Liquid.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Domain.Extensions.Crud.Queries.FindByIdGenericEntity
{
    /// <summary>
    /// Generic Handler to Find Entity By Id
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public class FindByIdGenericEntityQueryHandler<TEntity, TIdentifier> : IRequestHandler<FindByIdGenericEntityQuery<TEntity, TIdentifier>, FindByIdGenericEntityQueryResponse<TEntity>> where TEntity : LiquidEntity<TIdentifier>
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected readonly ILiquidRepository<TEntity, TIdentifier> _liquidRepository;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Initialize an instance of <see cref="FindByIdGenericEntityQueryHandler{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="liquidRepository"></param>
        public FindByIdGenericEntityQueryHandler(ILiquidRepository<TEntity, TIdentifier> liquidRepository)
        {
            _liquidRepository = liquidRepository;
        }

        /// <summary>
        /// Asynchronous virtual method that handles "find by id" requests
        /// </summary>
        /// <param name="request">"Find by id" request to handle of type <see cref="FindByIdGenericEntityQuery{TEntity, TIdentifier}"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="FindByIdGenericEntityQueryResponse{TEntity}"/></returns>
        public async Task<FindByIdGenericEntityQueryResponse<TEntity>> Handle(FindByIdGenericEntityQuery<TEntity, TIdentifier> request, CancellationToken cancellationToken)
        {
            var entity = await _liquidRepository.FindByIdAsync(request.Id);

            return new FindByIdGenericEntityQueryResponse<TEntity>(entity);
        }
    }
}