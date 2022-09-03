using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Cache
{
    /// <summary>
    /// Represents a distributed cache of serialized values.
    /// </summary>
    public interface ILiquidCache : IDistributedCache
    {
        /// <summary>
        /// Gets a value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The located value or null.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Gets a value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="token">Optional. The System.Threading.CancellationToken used to propagate notifications
        /// that the operation should be canceled.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, containing
        /// the located value or null.</returns>
        Task<T> GetAsync<T>(string key, CancellationToken token = default(CancellationToken));


        /// <summary>
        /// Sets a value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="value"> The value to set in the cache.</param>
        /// <param name="options">The cache options for the value.</param>
        void Set<T>(string key, T value, DistributedCacheEntryOptions options);

        /// <summary>
        /// Sets the value with the given key.
        /// </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="value">The value to set in the cache.</param>
        /// <param name="options">The cache options for the value.</param>
        /// <param name="token">Optional. The System.Threading.CancellationToken used to propagate notifications
        /// that the operation should be canceled.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation.</returns>
        Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken));

    }
}
