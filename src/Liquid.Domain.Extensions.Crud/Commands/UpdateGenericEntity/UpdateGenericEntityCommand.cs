using Liquid.Repository;
using MediatR;

namespace Liquid.Domain.Extensions.Crud.Commands.UpdateGenericEntity
{
    /// <summary>
    /// Generic Command to Update Entity
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public class UpdateGenericEntityCommand<TEntity, TIdentifier> : IRequest<UpdateGenericEntityCommandResponse<TEntity>> where TEntity : LiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Entity to Update
        /// </summary>
        public TEntity Data { get; private set; }

        /// <summary>
        /// Initialize an instance of <see cref="UpdateGenericEntityCommand{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="data">Entity to Update</param>
        public UpdateGenericEntityCommand(TEntity data)
        {
            Data = data;
        }
    }
}
