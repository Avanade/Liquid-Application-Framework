﻿using Liquid.Domain.Extensions.Crud.Notifications.GenericEntityRemoved;
using Liquid.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Domain.Extensions.Crud.Commands.RemoveGenericEntity
{
    /// <summary>
    /// Generic Handler to Remove Entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public class RemoveGenericEntityCommandHandler<TEntity, TIdentifier> : IRequestHandler<RemoveGenericEntityCommand<TEntity, TIdentifier>, RemoveGenericEntityCommandResponse<TEntity>> where TEntity : LiquidEntity<TIdentifier>
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected readonly ILiquidRepository<TEntity, TIdentifier> _liquidRepository;
        protected readonly IMediator _mediator;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Initialize an instance of <see cref="RemoveGenericEntityCommandHandler{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="liquidRepository">Instance of <see cref="ILiquidRepository{TEntity, TIdentifier}"/></param>
        /// <param name="mediator">Instance of <see cref="IMediator"/></param>
        public RemoveGenericEntityCommandHandler(ILiquidRepository<TEntity, TIdentifier> liquidRepository, IMediator mediator)
        {
            _liquidRepository = liquidRepository;
            _mediator = mediator;
        }

        /// <summary>
        /// Asynchronous virtual method that handles remove requests
        /// </summary>
        /// <param name="request">Remove request to handle of type <see cref="RemoveGenericEntityCommand{TEntity, TIdentifier}"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="RemoveGenericEntityCommandResponse{TEntity}"/></returns>
        public virtual async Task<RemoveGenericEntityCommandResponse<TEntity>> Handle(RemoveGenericEntityCommand<TEntity, TIdentifier> request, CancellationToken cancellationToken)
        {
            TEntity entity = await _liquidRepository.FindByIdAsync(request.Id);

            if (entity != null)
            {
                await _liquidRepository.RemoveByIdAsync(request.Id);
                await _mediator.Publish(new GenericEntityRemovedNotification<TEntity, TIdentifier>(entity), cancellationToken);
            }

            return new RemoveGenericEntityCommandResponse<TEntity>(entity);
        }
    }
}
