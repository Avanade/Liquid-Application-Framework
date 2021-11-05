namespace Liquid.Domain.Extensions.Crud.Queries.FindByIdGenericEntity
{
    /// <summary>
    /// Response of <see cref="FindByIdGenericEntityQuery{TEntity, TIdentifier}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class FindByIdGenericEntityQueryResponse<TEntity>
    {
        /// <summary>
        /// Entity that was found
        /// </summary>
        public TEntity Data { get; private set; }

        /// <summary>
        /// Initialize an instance of <see cref="FindByIdGenericEntityQueryResponse{TEntity}"/>
        /// </summary>
        /// <param name="data">Entity that was found</param>
        public FindByIdGenericEntityQueryResponse(TEntity data)
        {
            Data = data;
        }
    }
}
