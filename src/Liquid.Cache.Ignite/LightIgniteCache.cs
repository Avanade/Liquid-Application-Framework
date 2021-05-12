using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Expiry;
using Apache.Ignite.Core.Client;
using Liquid.Cache.Ignite.Configuration;
using Liquid.Core.Configuration;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;

namespace Liquid.Cache.Ignite
{
    /// <summary>
    /// Apache Ignite Implementation for ILightCache
    /// </summary>
    /// <seealso cref="Liquid.Cache.ILightCache" />
    public class LightIgniteCache : ILightCache
    {
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly ILightConfiguration<IgniteCacheSettings> _configuration;
        private readonly IIgniteClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightIgniteCache" /> class.
        /// </summary>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="configuration">The Apache Ignite configuration.</param>
        /// <exception cref="NullReferenceException">The ILoggerFactory interface is not initialized. Please initialize log in container</exception>
        public LightIgniteCache(ILightTelemetryFactory telemetryFactory, ILightConfiguration<IgniteCacheSettings> configuration)
        {
            _telemetryFactory = telemetryFactory;
            _configuration = configuration;
            _client = Connect();
        }

        /// <inheritdoc/>
        public async Task AddAsync<TObject>(string key, TObject obj, TimeSpan expirationDuration)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_Ignite");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(AddAsync)}_{key}");
                var cacheEntry = _client.GetOrCreateCache<string, object>(key).WithExpiryPolicy(new ExpiryPolicy(expirationDuration, null, null));
                await cacheEntry.PutAsync(key, obj);
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(AddAsync)}_{key}");
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Ignite");
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_Ignite");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(RemoveAsync)}_{key}");
                var cacheEntry = _client.GetOrCreateCache<string, object>(key);
                await cacheEntry.RemoveAsync(key);
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(RemoveAsync)}_{key}");
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Ignite");
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAllAsync()
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_Ignite");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(RemoveAllAsync)}");
                await Task.Run(() =>
                {
                    var keys = _client.GetCacheNames();
                    keys?.Each(key => _client.DestroyCache(key));
                });

                telemetry.CollectTelemetryStopWatchMetric($"{nameof(RemoveAllAsync)}");
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Ignite");
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
                    telemetry.AddContext("Cache_Ignite");
                    telemetry.StartTelemetryStopWatchMetric($"{nameof(GetAllKeysAsync)}_{pattern}");
                    var keys = _client.GetCacheNames().ToList();
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
                telemetry.RemoveContext("Cache_Ignite");
            }
            return returnKeys;
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                var returnValue = false;
                await Task.Run(() =>
                {
                    telemetry.AddContext("Cache_Ignite");
                    telemetry.StartTelemetryStopWatchMetric($"{nameof(ExistsAsync)}_{key}");
                    var cacheEntry = _client.GetOrCreateCache<string, object>(key);
                    returnValue = cacheEntry != null && cacheEntry.ContainsKey(key);
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
                telemetry.RemoveContext("Cache_Ignite");
            }
        }

        /// <inheritdoc/>
        public async Task<TObject> RetrieveAsync<TObject>(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_Ignite");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(RetrieveAsync)}_{key}");
                var cacheEntry = _client.GetOrCreateCache<string, object>(key);

                var returnValue = cacheEntry.ContainsKey(key) ? (TObject)await cacheEntry.GetAsync(key) : default;

                telemetry.CollectTelemetryStopWatchMetric($"{nameof(RetrieveAsync)}_{key}");
                return returnValue;
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Ignite");
            }
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns></returns>
        private IIgniteClient Connect()
        {
            var settings = _configuration?.Settings;

            if (settings == null) throw new LightCacheException("Apache Ignite settings does not exist, please check configuration.");

            try
            {
                var cfg = new IgniteClientConfiguration
                {
                    UserName = settings.Username,
                    Password = settings.Password,
                    SocketTimeout = TimeSpan.FromSeconds(settings.SocketTimeout ?? 5),
                    EnablePartitionAwareness = settings.EnablePartitionAwareness ?? false,
                    Endpoints = new List<string>()
                };

                settings.Servers?.Each(server => cfg.Endpoints.Add(server?.Address));

                return Ignition.StartClient(cfg);
            }
            catch (Exception ex)
            {
                throw new LightCacheException("Error connecting to Apache Ignite. See inner exception for more details.", ex);
            }
        }
    }
}