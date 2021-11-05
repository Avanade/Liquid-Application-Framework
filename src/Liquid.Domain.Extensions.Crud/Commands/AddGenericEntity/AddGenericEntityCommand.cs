using Liquid.Repository;
using MediatR;

namespace Liquid.Domain.Extensions.Crud.Commands.AddGenericEntity
{
    /// <summary>
    /// Generic Command to Add Entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public class AddGenericEntityCommand<TEntity, TIdentifier> : IRequest<AddGenericEntityCommandResponse<TEntity>> where TEntity : LiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Entity to Add
        /// </summary>
        public TEntity Data { get; private set; }

        /// <summary>
        /// Initialize an instance of <see cref="AddGenericEntityCommand{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="data"></param>
        public AddGenericEntityCommand(TEntity data)
        {
            Data = data;
        }
    }
}
