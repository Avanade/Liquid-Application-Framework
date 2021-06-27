using System;
using System.Collections.Generic;
using System.Text;

namespace Liquid.Core.Interfaces
{
    /// <summary>
    /// Manages notifications in the global context.
    /// </summary>
    public interface ILiquidContextNotifications
    {
        /// <summary>
        /// Global context service.
        /// </summary>
        ILiquidContext context { get; }

        /// <summary>
        /// Add or update a notification.
        /// </summary>
        /// <param name="key">Key of message.</param>
        /// <param name="value">Notification message.</param>
        void UpsertNotification(string key, object value);

        /// <summary>
        /// Add or update notifications list on context.
        /// </summary>
        /// <param name="notifications">Notifications list.</param>
        void UpsertNotifications(IDictionary<string, object> notifications);

        /// <summary>
        /// Gets notifications that exist in the global context.
        /// </summary>
        IDictionary<string, object> GetNotifications();
    }

}
