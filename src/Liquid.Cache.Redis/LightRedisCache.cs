using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Liquid.Cache.Redis.Configuration;
using Liquid.Core.Configuration;
using Liquid.Core.Telemetry;
using Liquid.Core.Utils;
using StackExchange.Redis;

namespace Liquid.Cache.Redis
{
    /// <summary>
    /// Redis Cache Implementation for ICache and IHashCache
    /// </summary>
    /// <seealso cref="Liquid.Cache.ILightCache" />
    public class LightRedisCache : ILightCache
    {
        private readonly ILightTelemetryFactory _telemetryFactory;
        private readonly ILightConfiguration<RedisCacheSettings> _redisConfiguration;
        private ConnectionMultiplexer _connection;
        private ConfigurationOptions _options;

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        private IDatabase Database
        {
            get
            {
                if (_connection == null || !_connection.IsConnected) { _connection = ConnectToRedis(); }
                return _connection.GetDatabase();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightRedisCache" /> class.
        /// </summary>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="redisConfiguration">The redis configuration.</param>
        /// <exception cref="NullReferenceException">The ILoggerFactory interface is not initialized. Please initialize log in container</exception>
        public LightRedisCache(ILightTelemetryFactory telemetryFactory, ILightConfiguration<RedisCacheSettings> redisConfiguration)
        {
            _telemetryFactory = telemetryFactory;
            _redisConfiguration = redisConfiguration;
            _connection = ConnectToRedis();
        }

        /// <inheritdoc/>
        public async Task AddAsync<TObject>(string key, TObject obj, TimeSpan expirationDuration)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_Redis");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(AddAsync)}_{key}");

                var jsonObj = obj.ToJsonBytes();
                await Database.StringSetAsync(key, jsonObj, expirationDuration);
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(AddAsync)}_{key}");
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Redis");
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_Redis");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(RemoveAsync)}_{key}");
                await Database.KeyDeleteAsync(key);
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(RemoveAsync)}_{key}");
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Redis");
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAllAsync()
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_Redis");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(RemoveAllAsync)}");
                var endpoints = _connection.GetEndPoints(true);
                foreach (var endpoint in endpoints)
                {
                    await _connection.GetServer(endpoint).FlushAllDatabasesAsync();
                }
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(RemoveAllAsync)}");
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Redis");
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> GetAllKeysAsync(string pattern = null)
        {
            if (_connection == null || !_connection.IsConnected) { _connection = ConnectToRedis(); }
            
            var telemetry = _telemetryFactory.GetTelemetry();
            var returnKeys = new List<string>();
            try
            {
                await Task.Run(() =>
                {
                    telemetry.AddContext("Cache_Redis");
                    telemetry.StartTelemetryStopWatchMetric($"{nameof(GetAllKeysAsync)}_{pattern}");
                    var endpoints = _connection.GetEndPoints(true);
                    foreach (var endpoint in endpoints)
                    {
                        var server = _connection.GetServer(endpoint);
                        returnKeys.AddRange(pattern == null
                                        ? server.Keys().Select(key => key.ToString())
                                        : server.Keys(pattern: pattern).Select(key => key.ToString()));
                    }
                    telemetry.CollectTelemetryStopWatchMetric($"{nameof(GetAllKeysAsync)}_{pattern}");
                });
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Redis");
            }
            return returnKeys;
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_Redis");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(ExistsAsync)}_{key}");
                var returnValue = await Database.KeyExistsAsync(key);
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(ExistsAsync)}_{key}");
                return returnValue;
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Redis");
            }
        }

        /// <inheritdoc/>
        public async Task<TObject> RetrieveAsync<TObject>(string key)
        {
            var telemetry = _telemetryFactory.GetTelemetry();
            try
            {
                telemetry.AddContext("Cache_Redis");
                telemetry.StartTelemetryStopWatchMetric($"{nameof(RetrieveAsync)}_{key}");
                var jsonBytes = await Database.StringGetAsync(key);
                telemetry.CollectTelemetryStopWatchMetric($"{nameof(RetrieveAsync)}_{key}");
                return jsonBytes.HasValue ? ((byte[])jsonBytes).ParseJson<TObject>() : default;
            }
            catch (Exception ex)
            {
                throw new LightCacheException(ex);
            }
            finally
            {
                telemetry.RemoveContext("Cache_Redis");
            }
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer ConnectToRedis()
        {
            var connectionString = _redisConfiguration?.Settings?.ConnectionString;

            if (connectionString.IsNullOrEmpty()) throw new LightCacheException("Redis connection string does not exist, please check configuration.");

            _options = ConfigurationOptions.Parse(connectionString);
            _options.AbortOnConnectFail = false;
            _options.ReconnectRetryPolicy = new LinearRetry(300);
            _options.ConnectRetry = 5;

            return ConnectionMultiplexer.Connect(_options);
        }
    }
}