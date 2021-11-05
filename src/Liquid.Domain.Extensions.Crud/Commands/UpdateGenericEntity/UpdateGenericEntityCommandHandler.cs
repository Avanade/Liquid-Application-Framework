using Liquid.Domain.Extensions.Crud.Notifications.GenericEntityUpdated;
using Liquid.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Domain.Extensions.Crud.Commands.UpdateGenericEntity
{
    /// <summary>
    /// Generic Handler to Update Entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public class UpdateGenericEntityCommandHandler<TEntity, TIdentifier> : IRequestHandler<UpdateGenericEntityCommand<TEntity, TIdentifier>, UpdateGenericEntityCommandResponse<TEntity>> where TEntity : LiquidEntity<TIdentifier>
    {
        protected readonly ILiquidRepository<TEntity, TIdentifier> _liquidRepository;
        protected readonly IMediator _mediator;

        /// <summary>
        /// Initialize an instance of <see cref="UpdateGenericEntityCommandHandler{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="liquidRepository">Instance of <see cref="ILiquidRepository{TEntity, TIdentifier}"/></param>
        /// <param name="mediator">Instance of <see cref="IMediator"/></param>
        public UpdateGenericEntityCommandHandler(ILiquidRepository<TEntity, TIdentifier> liquidRepository, IMediator mediator)
        {
            _liquidRepository = liquidRepository;
            _mediator = mediator;
        }

        public virtual async Task<UpdateGenericEntityCommandResponse<TEntity>> Handle(UpdateGenericEntityCommand<TEntity, TIdentifier> request, CancellationToken cancellationToken)
        {
            TEntity entity = await _liquidRepository.FindByIdAsync(request.Data.Id);

            if (entity != null)
            {
                await _liquidRepository.UpdateAsync(request.Data);
                await _mediator.Publish(new GenericEntityUpdatedNotification<TEntity, TIdentifier>(request.Data));
            }

            return new UpdateGenericEntityCommandResponse<TEntity>(entity);
        }
    }
}
