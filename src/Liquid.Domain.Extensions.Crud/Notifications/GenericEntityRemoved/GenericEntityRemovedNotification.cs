using Liquid.Repository;
using MediatR;

namespace Liquid.Domain.Extensions.Crud.Notifications.GenericEntityRemoved
{
    /// <summary>
    /// Notification of <see cref="GenericEntityRemovedNotification{TEntity, TIdentifier}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public class GenericEntityRemovedNotification<TEntity, TIdentifier> : INotification where TEntity : LiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Id of Entity Removed
        /// </summary>
        public TIdentifier Id => Data.Id;
        /// <summary>
        /// Entity Removed
        /// </summary>
        public TEntity Data { get; private set; }
        /// <summary>
        /// Nameof Entity Removed
        /// </summary>
        public string EntityName => Data.GetType().Name;

        /// <summary>
        /// Initialize an instance of <see cref="GenericEntityRemovedNotification{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="data">Entity Removed</param>
        public GenericEntityRemovedNotification(TEntity data)
        {
            Data = data;
        }
    }
}
