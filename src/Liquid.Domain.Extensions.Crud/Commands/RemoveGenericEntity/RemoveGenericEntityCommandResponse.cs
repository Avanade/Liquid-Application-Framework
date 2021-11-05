namespace Liquid.Domain.Extensions.Crud.Commands.RemoveGenericEntity
{
    /// <summary>
    /// Response of <see cref="RemoveGenericEntityCommand{TEntity, TIdentifier}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class RemoveGenericEntityCommandResponse<TEntity>
    {
        /// <summary>
        /// Entity to Remove
        /// </summary>
        public TEntity Data { get; private set; }

        /// <summary>
        /// Initialize an instance of <see cref="RemoveGenericEntityCommandResponse{TEntity}"/>
        /// </summary>
        /// <param name="data">Entity to Remove</param>
        public RemoveGenericEntityCommandResponse(TEntity data)
        {
            Data = data;
        }
    }
}
