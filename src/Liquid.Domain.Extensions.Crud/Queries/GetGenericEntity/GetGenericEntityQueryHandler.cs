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
        protected readonly ILiquidRepository<TEntity, TIdentifier> _liquidRepository;

        /// <summary>
        /// Initialize an instance of <see cref="FindByIdGenericEntityQueryHandler{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="liquidRepository"></param>
        public FindByIdGenericEntityQueryHandler(ILiquidRepository<TEntity, TIdentifier> liquidRepository)
        {
            _liquidRepository = liquidRepository;
        }

        public async Task<FindByIdGenericEntityQueryResponse<TEntity>> Handle(FindByIdGenericEntityQuery<TEntity, TIdentifier> request, CancellationToken cancellationToken)
        {
            var entity = await _liquidRepository.FindByIdAsync(request.Id);

            return new FindByIdGenericEntityQueryResponse<TEntity>(entity);
        }
    }
}