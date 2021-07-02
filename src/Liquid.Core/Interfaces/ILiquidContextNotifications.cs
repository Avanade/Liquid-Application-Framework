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
        /// Add or update a notification message.
        /// </summary>
        /// <param name="message">Message text.</param>
        void InsertNotification(string message);

        /// <summary>
        /// Add or update notifications list on context.
        /// </summary>
        /// <param name="notifications">Notification messages list.</param>
        void InsertNotifications(IList<string> notifications);

        /// <summary>
        /// Gets notification messages that exist in the global context.
        /// </summary>
        IList<string> GetNotifications();
    }

}
