using Liquid.Repository;
using MediatR;

namespace Liquid.Domain.Extensions.Crud.Notifications.GenericEntityAdded
{
    /// <summary>
    /// Notification of <see cref="GenericEntityAddedNotification{TEntity, TIdentifier}"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public class GenericEntityAddedNotification<TEntity, TIdentifier> : INotification where TEntity : LiquidEntity<TIdentifier>
    {
        /// <summary>
        /// Id of Entity Added
        /// </summary>
        public TIdentifier Id => Data.Id;
        /// <summary>
        /// Entity Added
        /// </summary>
        public TEntity Data { get; private set; }
        /// <summary>
        /// Nameof Entity Added
        /// </summary>
        public string EntityName => Data.GetType().Name;

        /// <summary>
        /// Initialize an instance of <see cref="GenericEntityAddedNotification{TEntity, TIdentifier}"/>
        /// </summary>
        /// <param name="data">Entity Added</param>
        public GenericEntityAddedNotification(TEntity data)
        {
            Data = data;
        }
    }
}
