using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Cache
{
    /// <summary>
    /// Light Cache interface extensions methods.
    /// </summary>
    public static class ILightCacheExtensions
    {
        /// <summary>
        /// Retrieves the specified object from cache, if the object does not exist, executes the action and adds the result to cache.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="key">The cache entry key.</param>
        /// <param name="action">The action to be executed to add the object to cache.</param>
        /// <param name="expirationDuration">Duration of the expiration.</param>
        /// <returns>the object in cache.</returns>
        public static async Task<TObject> RetrieveOrAddAsync<TObject>(this ILightCache cache, string key, Func<TObject> action, TimeSpan expirationDuration)
        {
            var result = await cache.RetrieveAsync<TObject>(key);
            if (result != null)
            {
                return result;
            }
            var obj = action.Invoke();
            await cache.AddAsync(key, obj, expirationDuration);
            return obj;

        }

        /// <summary>
        /// Retrieves the specified object from cache, if the object does not exist, executes the asynchronous action and adds the result to cache.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="key">The cache entry key.</param>
        /// <param name="action">The action to be executed to add the object to cache.</param>
        /// <param name="expirationDuration">Duration of the expiration.</param>
        /// <returns>the object in cache.</returns>
        public static async Task<TObject> RetrieveOrAddAsync<TObject>(this ILightCache cache, string key, Func<Task<TObject>> action, TimeSpan expirationDuration)
        {
            var result = await cache.RetrieveAsync<TObject>(key);
            if (result != null)
            {
                return result;
            }
            var obj = await action.Invoke();
            await cache.AddAsync(key, obj, expirationDuration);
            return obj;
        }
    }
}
