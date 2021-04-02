using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Liquid.Cache.Memory
{
    /// <summary>
    /// Implements the Microsoft.Extensions.Caching.Memory MemoryCache operations.
    /// </summary>
    /// <seealso cref="Liquid.Cache.ILightCache" />
    public class LightMemoryCache : ILightCache
    {
        private static CancellationTokenSource _resetCacheToken = new CancellationTokenSource();
        private readonly MemoryCache _cache;
        private readonly ILightTelemetryFactory _telemetryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightMemoryCache"/> class.
        /// </summary>
        public LightMemoryCache(ILightTelemetryFactory telemetryFactory)
        {
            try
            {
                _telemetryFactory = telemetryFactory;
                _cache = new MemoryCache(new MemoryCacheOptions());
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
        }

        /// <summary>
        /// Adds the specified object to cache.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="key">The cache entry key.</param>
        /// <param name="obj">The object.</param>
        /// <param name="expirationDuration">Duration of the expiration.</param>
        public async Task AddAsync<TObject>(string key, TObject obj, TimeSpan expirationDuration)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                await Task.Run(() =>
                {
                    telemetry.AddContext("Cache_Memory");
                    telemetry.StartTelemetryStopWatchMetric($"{nameof(AddAsync)}_{key}");

                    var options = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.Normal).SetAbsoluteExpiration(DateTimeOffset.Now.Add(expirationDuration));
                    options.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
                    _cache.Set(key, obj, options);
                    telemetry.CollectTelemetryStopWatchMetric($"{nameof(AddAsync)}_{key}");
                });
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Memory");
            }
        }

        /// <summary>
        /// Removes the specified cache entry.
        /// </summary>
        /// <param name="key">The key.</param>
        public async Task RemoveAsync(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_Memory");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(RemoveAsync)}_{key}");
                await Task.Run(() => _cache.Remove(key));
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(RemoveAsync)}_{key}");
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Memory");
            }
        }

        /// <summary>
        /// Removes all cache entries.
        /// </summary>
        public async Task RemoveAllAsync()
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                await Task.Run(() =>
                {
                    telemetry.AddContext("Cache_Memory");
                    telemetry.StartTelemetryStopWatchMetric($"{nameof(RemoveAllAsync)}");
                    if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested && _resetCacheToken.Token.CanBeCanceled)
                    {
                        _resetCacheToken.Cancel();
                        _resetCacheToken.Dispose();
                    }
                    _resetCacheToken = new CancellationTokenSource();
                    telemetry.CollectTelemetryStopWatchMetric($"{nameof(RemoveAllAsync)}");
                });
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Memory");
            }
        }

        /// <summary>
        /// Returns all keys from cache.
        /// </summary>
        /// <param name="pattern">the search pattern to return only keys that satisfies the condition.</param>
        /// <returns></returns>
        /// <exception cref="LightCacheException"></exception>
        public async Task<IEnumerable<string>> GetAllKeysAsync(string pattern = null)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            var entriesCollectionPropertyInfo = _cache.GetType().GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var returnKeys = new List<string>();

            try
            {
                await Task.Run(() =>
                {
                    telemetry.AddContext("Cache_Memory");
                    telemetry.StartTelemetryStopWatchMetric($"{nameof(GetAllKeysAsync)}_{pattern}");
                    
                    if (!(entriesCollectionPropertyInfo?.GetValue(_cache) is ICollection collection)) return;
                    
                    var keysCollection = (ReadOnlyCollection<object>)collection.ToDictionary()["Keys"];
                    var keys = pattern == null ? keysCollection.Cast<string>() : keysCollection.Cast<string>().Where(k => k.Contains(pattern));

                    returnKeys.AddRange(keys);
                    telemetry.CollectTelemetryStopWatchMetric($"{nameof(GetAllKeysAsync)}_{pattern}");
                });
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Memory");
            }
            return returnKeys;
        }

        /// <summary>
        /// Check if cache entry key exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                return await Task.Run(() =>
                {
                    telemetry.AddContext("Cache_Memory");
                    telemetry.StartTelemetryStopWatchMetric($"{nameof(ExistsAsync)}_{key}");
                    var returnValue = _cache.TryGetValue(key, out _);
                    telemetry.CollectTelemetryStopWatchMetric($"{nameof(ExistsAsync)}_{key}");
                    return returnValue;
                });
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Memory");
            }
        }

        /// <summary>
        /// Retrieves the specified object from cache.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="key">The cache entry key.</param>
        /// <returns>
        /// the object in cache.
        /// </returns>
        public async Task<TObject> RetrieveAsync<TObject>(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                return await Task.Run(() =>
                {
                    telemetry.AddContext("Cache_Memory");
                    telemetry.StartTelemetryStopWatchMetric($"{nameof(RetrieveAsync)}_{key}");
                    var obj = _cache.Get<TObject>(key);
                    telemetry.CollectTelemetryStopWatchMetric($"{nameof(RetrieveAsync)}_{key}");
                    return obj;
                });
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Memory");
            }
        }

        /// <summary>
        /// Retrieves the specified object from cache, if the object does not exist, adds the result.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="key">The cache entry key.</param>
        /// <param name="action">The action to be executed to add the object to cache.</param>
        /// <param name="expirationDuration">Duration of the expiration.</param>
        /// <returns>
        /// the object in cache.
        /// </returns>
        public async Task<TObject> RetrieveOrAddAsync<TObject>(string key, Func<TObject> action, TimeSpan expirationDuration)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_Memory");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(RetrieveOrAddAsync)}_{key}");
                var obj = _cache.Get<TObject>(key);
                if (obj == null)
                {
                    obj = action.Invoke();
                    await AddAsync(key, obj, expirationDuration);
                }
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(RetrieveOrAddAsync)}_{key}");
                return obj;
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Memory");
            }
        }
    }
}