using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.Messaging.Configuration;
using Liquid.Messaging.Exceptions;

namespace Liquid.Messaging.Extensions
{
    /// <summary>
    /// Event Setting Extensions Class.
    /// </summary>
    public static class MessagingSettingsExtensions
    {
        /// <summary>
        /// Gets the event source setting by id.
        /// </summary>
        /// <param name="connectionSettings">The event settings.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static MessagingSettings GetMessagingSettings(this List<MessagingSettings> connectionSettings, string id)
        {
            var setting = connectionSettings.FirstOrDefault(cs => string.Equals(cs.Id, id, StringComparison.CurrentCultureIgnoreCase));
            if (setting == null) throw new MessagingMissingConfigurationException(id);
            return setting;
        }

        /// <summary>
        /// Gets the event custom parameter value. 
        /// </summary>
        /// <typeparam name="TParameterType">The type of the parameter value. Allowed types: string, int, long, boolean, double, timespan
        /// </typeparam>
        /// <param name="parameters">The parameter list.</param>
        /// <param name="key">The parameter key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// the parameter value in <typeparam>
        ///     <name>TParameterType</name>
        /// </typeparam>
        /// type.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Key type invalid or nonexistent key.
        /// </exception>
        public static TParameterType GetCustomParameter<TParameterType>(this List<CustomParameter> parameters, string key, object defaultValue = null)
        {
            if ((parameters == null || !parameters.Any()) && defaultValue != null)
            {
                return (TParameterType)Convert.ChangeType(defaultValue, typeof(TParameterType));
            }

            var parameter = parameters?.FirstOrDefault(p => key.Equals(p.Key, StringComparison.InvariantCultureIgnoreCase));
            if (parameter != null)
            {
                var objectValue = parameter.Value;
                switch (typeof(TParameterType).Name.ToLowerInvariant())
                {
                    case "string":
                    case "int32":
                    case "int64":
                    case "boolean":
                    case "double":
                        return (TParameterType)Convert.ChangeType(objectValue, typeof(TParameterType));
                    case "timespan":
                        TimeSpan.TryParse((string)objectValue, out var timespan);
                        return (TParameterType)Convert.ChangeType(timespan, typeof(TParameterType));
                }
                throw new ArgumentOutOfRangeException(nameof(key), $"{key} key type error, expected {typeof(TParameterType)}. Sent {parameter.Value.GetType()}");
            }

            if (defaultValue != null)
            {
                return (TParameterType)Convert.ChangeType(defaultValue, typeof(TParameterType));
            }

            throw new ArgumentOutOfRangeException(nameof(key), $"{key} key Not found in parameters collection.");
        }
    }
}