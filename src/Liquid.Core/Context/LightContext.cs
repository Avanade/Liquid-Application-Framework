using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;

namespace Liquid.Core.Context
{
    /// <summary>
    /// Liquid Global context for micro-services.
    /// </summary>
    /// <seealso cref="Liquid.Core.Context.ILightContext" />
    public class LightContext : ILightContext
    {
        private const string ContextCorrelationIdKey = "ContextCorrelationIdKey";
        private const string BusinessCorrelationIdKey = "BusinessCorrelationIdKey";
        private const string ContextCultureKey = "ContextCultureKey";
        private const string ContextChannelKey = "ContextChannelKey";
        private readonly ContextData _contextData;
        private readonly ContextData _contextNotifications;

        /// <summary>
        /// Gets the context identifier.
        /// </summary>
        /// <value>
        /// The context identifier.
        /// </value>
        public Guid ContextId => GetContextDataValue<Guid>(ContextCorrelationIdKey);

        /// <summary>
        /// Gets the business context identifier.
        /// </summary>
        /// <value>
        /// The business context identifier.
        /// </value>
        public Guid BusinessContextId => GetContextDataValue<Guid>(BusinessCorrelationIdKey);

        /// <summary>
        /// Gets the culture info of context.
        /// </summary>
        /// <value>
        /// The culture info.
        /// </value>
        public string ContextCulture => GetContextDataValue<string>(ContextCultureKey);
        
        /// <summary>
        /// Gets the context channel.
        /// </summary>
        /// <value>
        /// The context channel.
        /// </value>
        public string ContextChannel => GetContextDataValue<string>(ContextChannelKey);

        /// <summary>
        /// Initializes a new instance of the <see cref="LightContext"/> class.
        /// </summary>
        public LightContext()
        {
            _contextData = new ContextData();
            _contextNotifications = new ContextData();
            AddOrReplaceContextValue(ContextCorrelationIdKey, Guid.NewGuid());
            AddOrReplaceContextValue(BusinessCorrelationIdKey, Guid.NewGuid());
            AddOrReplaceContextValue(ContextCultureKey, Thread.CurrentThread.CurrentCulture.Name);
        }

        /// <summary>
        /// Gets the context data value.
        /// </summary>
        /// <typeparam name="TDataValue">The type of the data value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TDataValue GetContextDataValue<TDataValue>(string key)
        {
            if (_contextData.ContainsKey(key))
            {
                return (TDataValue)_contextData[key];
            }
            return default;
        }

        /// <summary>
        /// Adds the data value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddOrReplaceContextValue(string key, object value)
        {
            _contextData.AddOrReplace(key, value);
        }

        /// <summary>
        /// Sets a new context identifier.
        /// </summary>
        /// <param name="id">The new context identifier.</param>
        /// <exception cref="ArgumentOutOfRangeException">id</exception>
        public void SetContextId(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentOutOfRangeException(nameof(id));
            _contextData.AddOrReplace(ContextCorrelationIdKey, id);
        }

        /// <summary>
        /// Sets the new business context identifier.
        /// </summary>
        /// <param name="id">The new business context identifier.</param>
        /// <exception cref="ArgumentOutOfRangeException">id</exception>
        public void SetBusinessContextId(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentOutOfRangeException(nameof(id));
            _contextData.AddOrReplace(BusinessCorrelationIdKey, id);
        }

        /// <summary>
        /// Sets the culture.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        public void SetCulture(string cultureName)
        {
            var cultureInfo = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            _contextData.AddOrReplace(ContextCultureKey, cultureName);
        }

        /// <summary>
        /// Sets the channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public void SetChannel(string channel)
        {
            _contextData.AddOrReplace(ContextChannelKey, channel);
        }

        /// <summary>
        /// Adds a notification to context.
        /// </summary>
        /// <param name="key">The notification key.</param>
        /// <param name="message">The notification message.</param>
        public void Notify(string key, string message)
        {
            _contextNotifications.AddOrReplace(key, message);
        }

        /// <summary>
        /// Gets the notification.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetNotification(string key)
        {
            return _contextNotifications.TryGetValue(key, out var result) ? result.ToString() : null;
        }

        /// <summary>
        /// Gets the context notifications.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetNotifications()
        {
            return _contextNotifications;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"ContextId: {ContextId}, BusinessContextId: {BusinessContextId}, ContextCulture: {ContextCulture}, ContextChannel: {ContextChannel}";
        }
    }
}