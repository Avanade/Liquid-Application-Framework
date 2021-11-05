using Liquid.Repository;
using MediatR;

namespace Liquid.Domain.Extensions.Crud.Notifications.GenericEntityUpdated
{
    /// <summary>
    /// Notification of <see cref="GenericEntityUpdated{TEntity, TIdentifier}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public class GenericEntityUpdatedNotification<TEntity, TIdentifier> : INotification where TEntity : LiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Id of Entity Updated
        /// </summary>
        public TIdentifier Id => Data.Id;
        /// <summary>
        /// Entity Updated
        /// </summary>
        public TEntity Data { get; private set; }
        /// <summary>
        /// Nameof Entity Updated
        /// </summary>
        public string EntityName => Data.GetType().Name;

        /// <summary>
        /// Initialize an instance of <see cref="GenericEntityUpdatedNotification{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="data">Entity Updated</param>
        public GenericEntityUpdatedNotification(TEntity data)
        {
            Data = data;
        }
    }
}
