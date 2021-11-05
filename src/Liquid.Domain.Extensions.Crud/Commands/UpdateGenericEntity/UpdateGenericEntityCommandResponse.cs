namespace Liquid.Domain.Extensions.Crud.Commands.UpdateGenericEntity
{
    /// <summary>
    /// Response of <see cref="UpdateGenericEntityCommand{TEntity, TIdentifier}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class UpdateGenericEntityCommandResponse<TEntity>
    {
        /// <summary>
        /// Gets an instance of your entity./>
        /// </summary>
        public TEntity Data { get; private set; }

        /// <summary>
        /// Initialize an instance of <see cref="UpdateGenericEntityCommandResponse{TEntity}"/>
        /// </summary>
        /// <param name="data"></param>
        public UpdateGenericEntityCommandResponse(TEntity data)
        {
            Data = data;
        }
    }
}
