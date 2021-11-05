using Liquid.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Domain.Extensions.Crud.Queries.GetAllGenericEntity
{
    /// <summary>
    /// Generic Handler to Get All Entities
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>

    public class GetAllGenericEntityQueryHandler<TEntity, TIdentifier> : IRequestHandler<GetAllGenericEntityQuery<TEntity, TIdentifier>, GetAllGenericEntityQueryResponse<TEntity>> where TEntity : LiquidEntity<TIdentifier>
    {
        protected readonly ILiquidRepository<TEntity, TIdentifier> _liquidRepository;

        /// <summary>
        /// Initialize an instance of <see cref="GetAllGenericEntityQueryHandler{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="liquidRepository">Instance of <see cref="ILiquidRepository{TEntity, TIdentifier}"</param>
        public GetAllGenericEntityQueryHandler(ILiquidRepository<TEntity, TIdentifier> liquidRepository)
        {
            _liquidRepository = liquidRepository;
        }

        public async Task<GetAllGenericEntityQueryResponse<TEntity>> Handle(GetAllGenericEntityQuery<TEntity, TIdentifier> request, CancellationToken cancellationToken)
        {
            var entities = await _liquidRepository.FindAllAsync();

            return new GetAllGenericEntityQueryResponse<TEntity>(entities);
        }
    }
}