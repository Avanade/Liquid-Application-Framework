using Liquid.Repository;
using MediatR;

namespace Liquid.Domain.Extensions.Crud.Commands.RemoveGenericEntity
{
    /// <summary>
    /// Generic Command to Remove Entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>

    public class RemoveGenericEntityCommand<TEntity, TIdentifier> : IRequest<RemoveGenericEntityCommandResponse<TEntity>> where TEntity : LiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Id of Entity
        /// </summary>
        public TIdentifier Id { get; set; }

        /// <summary>
        /// Initialize an instance of <see cref="RemoveGenericEntityCommand{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="id">Id of Entity to Remove</param>
        public RemoveGenericEntityCommand(TIdentifier id)
        {
            Id = id;
        }
    }
}