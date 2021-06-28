using Liquid.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Core.Implementations
{
    ///<inheritdoc/>
    public class LiquidContextNotifications : ILiquidContextNotifications
    {
        private Guid notificationKey = Guid.NewGuid();
        private readonly ILiquidContext _liquidContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="liquidContext"></param>
        public LiquidContextNotifications(ILiquidContext liquidContext)
        {
            _liquidContext = liquidContext;
        }

        ///<inheritdoc/>
        public ILiquidContext context => _liquidContext;

        ///<inheritdoc/>
        public void UpsertNotification(string key, object value)
        {
            var notifications = (Dictionary<string, object>)_liquidContext.Get(notificationKey.ToString());

            if (notifications is null)
            {
                notifications = new Dictionary<string, object>();
                notifications.Add(key, value);
            }
            else
            {
                if (notifications.ContainsKey(key))
                {
                    notifications[key] = value;
                }
                else
                {
                    notifications.TryAdd(key, value);
                }
            } 
            
            UpsertNotifications(notifications);
        }

        ///<inheritdoc/>
        public void UpsertNotifications(IDictionary<string, object> notifications)
        {
            _liquidContext.Upsert(notificationKey.ToString(), notifications);
        }

        ///<inheritdoc/>
        public IDictionary<string, object> GetNotifications()
        {
            try
            {
                return (IDictionary<string, object>)_liquidContext.current[notificationKey.ToString()];
            }
            catch
            {
                return null;
            }
            
        }
    }
}
