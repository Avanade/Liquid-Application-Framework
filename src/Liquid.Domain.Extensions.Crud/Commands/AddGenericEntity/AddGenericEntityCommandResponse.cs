using Liquid.Repository;

namespace Liquid.Domain.Extensions.Crud.Commands.AddGenericEntity
{
    /// <summary>
    /// Response of <see cref="AddGenericEntityCommand{TEntity, TIdentifier}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class AddGenericEntityCommandResponse<TEntity>
    {
        /// <summary>
        /// Entity Added
        /// </summary>
        public TEntity Data { get; private set; }

        /// <summary>
        /// Initialize an instance of <see cref="AddGenericEntityCommandResponse{TEntity}"/>
        /// </summary>
        /// <param name="data">Entity to Add</param>
        public AddGenericEntityCommandResponse(TEntity data)
        {
            Data = data;
        }
    }
}
