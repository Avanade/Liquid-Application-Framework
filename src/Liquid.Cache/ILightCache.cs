using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Liquid.Cache
{
    /// <summary>
    /// Interface for cache functionality.
    /// </summary>
    public interface ILightCache
    {
        /// <summary>
        /// Adds the specified object to cache.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="key">The cache entry key.</param>
        /// <param name="obj">The object.</param>
        /// <param name="expirationDuration">Duration of the expiration.</param>
        Task AddAsync<TObject>(string key, TObject obj, TimeSpan expirationDuration);

        /// <summary>
        /// Removes the specified cache entry.
        /// </summary>
        /// <param name="key">The key.</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Removes all cache entries.
        /// </summary>
        Task RemoveAllAsync();

        /// <summary>
        /// Check if cache entry key exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Returns all keys from cache.
        /// </summary>
        /// <param name="pattern">the search pattern to return only keys that satisfies the condition.</param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetAllKeysAsync(string pattern = null);

        /// <summary>
        /// Retrieves the specified object from cache.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="key">The cache entry key.</param>
        /// <returns>the object in cache.</returns>
        Task<TObject> RetrieveAsync<TObject>(string key);
    }
}
