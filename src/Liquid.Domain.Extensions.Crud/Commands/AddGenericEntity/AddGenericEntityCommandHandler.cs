using Liquid.Domain.Extensions.Crud.Notifications.GenericEntityAdded;
using Liquid.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Domain.Extensions.Crud.Commands.AddGenericEntity
{
    /// <summary>
    /// Generic Handler to Add Entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public class AddGenericEntityCommandHandler<TEntity, TIdentifier> : IRequestHandler<AddGenericEntityCommand<TEntity, TIdentifier>, AddGenericEntityCommandResponse<TEntity>> where TEntity : LiquidEntity<TIdentifier>
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected readonly ILiquidRepository<TEntity, TIdentifier> _liquidRepository;
        protected readonly IMediator _mediator;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Initialize an instance of <see cref="AddGenericEntityCommandHandler{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="liquidRepository">Instance of <see cref="ILiquidRepository{TEntity, TIdentifier}"/></param>
        /// <param name="mediator">Instance of <see cref="IMediator"/></param>
        public AddGenericEntityCommandHandler(ILiquidRepository<TEntity, TIdentifier> liquidRepository, IMediator mediator)
        {
            _liquidRepository = liquidRepository;
            _mediator = mediator;
        }

        /// <summary>
        /// Asynchronous virtual method that handles add requests
        /// </summary>
        /// <param name="request">Add request to handle of type <see cref="AddGenericEntityCommand{TEntity, TIdentifier}"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="AddGenericEntityCommandResponse{TEntity}"/></returns>
        public virtual async Task<AddGenericEntityCommandResponse<TEntity>> Handle(AddGenericEntityCommand<TEntity, TIdentifier> request, CancellationToken cancellationToken)
        {
            await _liquidRepository.AddAsync(request.Data);

            await _mediator.Publish(new GenericEntityAddedNotification<TEntity, TIdentifier>(request.Data));

            return new AddGenericEntityCommandResponse<TEntity>(request.Data);
        }
    }
}
