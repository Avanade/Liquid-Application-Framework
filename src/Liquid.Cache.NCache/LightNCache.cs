using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Liquid.Cache.NCache.Configuration;
using Liquid.Core.Configuration;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;

namespace Liquid.Cache.NCache
{
    /// <summary>
    /// NCache Implementation for ILightCache
    /// </summary>
    /// <seealso cref="Liquid.Cache.ILightCache" />
    public class LightNCache : ILightCache
    {
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly ILightConfiguration<NCacheSettings> _configuration;
        private readonly ICache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightNCache" /> class.
        /// </summary>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="configuration">The NCache configuration.</param>
        /// <exception cref="NullReferenceException">The ILoggerFactory interface is not initialized. Please initialize log in container</exception>
        public LightNCache(ILightTelemetryFactory telemetryFactory, ILightConfiguration<NCacheSettings> configuration)
        {
            _telemetryFactory = telemetryFactory;
            _configuration = configuration;
            _cache = Connect();
        }

        /// <inheritdoc/>
        public async Task AddAsync<TObject>(string key, TObject obj, TimeSpan expirationDuration)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_NCache");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(AddAsync)}_{key}");

                var cacheItem = new CacheItem(obj) { Expiration = new Expiration(ExpirationType.Absolute, expirationDuration) };
                await _cache.InsertAsync(key, cacheItem);
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(AddAsync)}_{key}");
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_NCache");
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_NCache");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(RemoveAsync)}_{key}");
                await _cache.RemoveAsync<object>(key);
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(RemoveAsync)}_{key}");
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_NCache");
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAllAsync()
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_NCache");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(RemoveAllAsync)}");
                await Task.Run(() => _cache.Clear());
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(RemoveAllAsync)}");
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_NCache");
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> GetAllKeysAsync(string pattern = null)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            var returnKeys = new List<string>();
            try
            {
                await Task.Run(() =>
                {
                    telemetry.AddContext("Cache_NCache");
                    telemetry.StartTelemetryStopWatchMetric($"{nameof(GetAllKeysAsync)}_{pattern}");

                    var keys = new List<string>();
                    keys.AddRange(_cache.Cast<DictionaryEntry>().Select(cacheEntry => cacheEntry.Key.ToString()));
                    returnKeys.AddRange(pattern == null ? keys : keys.Where(k => k.Contains(pattern)));

                    telemetry.CollectTelemetryStopWatchMetric($"{nameof(GetAllKeysAsync)}_{pattern}");
                });
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_NCache");
            }
            return returnKeys;
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            var keys = new List<string>();
            try
            {
                var returnValue = false;
                await Task.Run(() =>
                {
                    telemetry.AddContext("Cache_NCache");
                    telemetry.StartTelemetryStopWatchMetric($"{nameof(ExistsAsync)}_{key}");
                    keys.AddRange(_cache.Cast<DictionaryEntry>().Select(cacheEntry => cacheEntry.Key.ToString()));
                    returnValue = keys.Contains(key);
                    telemetry.CollectTelemetryStopWatchMetric($"{nameof(ExistsAsync)}_{key}");
                });
                return returnValue;
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_NCache");
            }
        }

        /// <inheritdoc/>
        public async Task<TObject> RetrieveAsync<TObject>(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                TObject returnValue = default;
                await Task.Run(() =>
                {
                    telemetry.AddContext("Cache_NCache");
                    telemetry.StartTelemetryStopWatchMetric($"{nameof(RetrieveAsync)}_{key}");
                    returnValue = _cache.Get<TObject>(key);
                    telemetry.CollectTelemetryStopWatchMetric($"{nameof(RetrieveAsync)}_{key}");
                });
                return returnValue;
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_NCache");
            }
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns></returns>
        private ICache Connect()
        {
            var settings = _configuration?.Settings;

            if (settings == null) throw new LightCacheException("NCache settings does not exist, please check configuration.");

            try
            {
                var options = new CacheConnectionOptions
                {
                    RetryInterval = TimeSpan.FromSeconds(settings.RetryInterval ?? 5),
                    ConnectionRetries = settings.ConnectionRetries ?? 5,
                    EnableKeepAlive = settings.EnableKeepAlive ?? true,
                    KeepAliveInterval = TimeSpan.FromSeconds(settings.KeepAliveInterval ?? 30),
                    ServerList = new List<ServerInfo>()
                };

                settings.Servers?.Each(server => options.ServerList.Add(new ServerInfo(server?.Address, server?.Port ?? 9800)));

                // Connect to cache with CacheConnectionOptions
                return CacheManager.GetCache(settings.Name, options);
            }
            catch (Exception ex)
            {
                throw new LightCacheException("Error connecting to NCache. See inner exception for more details.", ex);
            }
        }
    }
}