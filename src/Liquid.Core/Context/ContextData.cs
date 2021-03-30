using System.Collections.Concurrent;

namespace Liquid.Core.Context
{
    /// <summary>
    /// Class responsible to hold the context data of current scope context.
    /// </summary>
    /// <seealso>
    ///     <cref>System.Collections.Generic.Dictionary{System.String, System.Object}</cref>
    /// </seealso>
    public class ContextData : ConcurrentDictionary<string, object>
    {
        /// <summary>
        /// Adds the or replace data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddOrReplace(string key, object value)
        {
            if (ContainsKey(key))
            {
                this[key] = value;
            }
            else
            {
                TryAdd(key, value);
            }
        }
    }
}