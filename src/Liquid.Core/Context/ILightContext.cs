using System;
using System.Collections.Generic;

namespace Liquid.Core.Context
{
    /// <summary>
    /// Global Context for Micro-service.
    /// </summary>
    public interface ILightContext
    {
        /// <summary>
        /// Gets the context identifier.
        /// </summary>
        /// <value>
        /// The context identifier.
        /// </value>
        Guid ContextId { get; }

        /// <summary>
        /// Gets the business context identifier.
        /// </summary>
        /// <value>
        /// The business context identifier.
        /// </value>
        Guid BusinessContextId { get; }

        /// <summary>
        /// Gets the culture info of context.
        /// </summary>
        /// <value>
        /// The culture info.
        /// </value>
        string ContextCulture { get; }

        /// <summary>
        /// Gets the context channel.
        /// </summary>
        /// <value>
        /// The context channel.
        /// </value>
        string ContextChannel { get; }

        /// <summary>
        /// Gets the context data value.
        /// </summary>
        /// <typeparam name="TDataValue">The type of the data value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        TDataValue GetContextDataValue<TDataValue>(string key);

        /// <summary>
        /// Adds the data value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void AddOrReplaceContextValue(string key, object value);

        /// <summary>
        /// Sets a new context identifier.
        /// </summary>
        /// <param name="id">The new context identifier.</param>
        void SetContextId(Guid id);

        /// <summary>
        /// Sets the new business context identifier.
        /// </summary>
        /// <param name="id">The new business context identifier.</param>
        void SetBusinessContextId(Guid id);

        /// <summary>
        /// Sets the culture.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        void SetCulture(string cultureName);

        /// <summary>
        /// Sets the channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        void SetChannel(string channel);

        /// <summary>
        /// Adds a notification to context.
        /// </summary>
        /// <param name="key">The notification key.</param>
        /// <param name="message">The notification message.</param>
        void Notify(string key, string message);

        /// <summary>
        /// Gets the notification.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string GetNotification(string key);

        /// <summary>
        /// Gets the context notifications.
        /// </summary>
        /// <returns></returns>
        IDictionary<string, object> GetNotifications();
    }
}