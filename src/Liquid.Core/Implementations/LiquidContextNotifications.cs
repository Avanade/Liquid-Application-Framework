using Liquid.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Core.Implementations
{
    ///<inheritdoc/>
    public class LiquidContextNotifications : ILiquidContextNotifications
    {
        private readonly string _notificationKey = "notification_" + Guid.NewGuid();
        private readonly ILiquidContext _liquidContext;

        /// <summary>
        /// Initialize an instance of <seealso cref="LiquidContextNotifications"/>
        /// </summary>
        /// <param name="liquidContext"></param>
        public LiquidContextNotifications(ILiquidContext liquidContext)
        {
            _liquidContext = liquidContext;
        }

        ///<inheritdoc/>
        public void InsertNotification(string message)
        {
            var notifications = _liquidContext.Get<IList<string>>(_notificationKey);
            if (notifications is null)
                notifications = new List<string>();

            notifications.Add(message);

            _liquidContext.Upsert(_notificationKey, notifications);
        }

        ///<inheritdoc/>
        public void InsertNotifications(IList<string> notifications)
        {
            _liquidContext.Upsert(_notificationKey, notifications);
        }

        ///<inheritdoc/>
        public IList<string> GetNotifications()
        {
            try
            {
                return _liquidContext.Get<IList<string>>(_notificationKey);
            }
            catch
            {
                return null;
            }
        }
    }
}
