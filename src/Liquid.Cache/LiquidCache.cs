using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Liquid.Cache
{
    ///<inheritdoc/>
    public class LiquidCache : ILiquidCache
    {
        private readonly IDistributedCache _distributedCache;

        /// <summary>
        /// Initialize a new instance of <see cref="LiquidCache"/>
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public LiquidCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        ///<inheritdoc/>
        public byte[] Get(string key)
        {
            return _distributedCache.Get(key);
        }

        ///<inheritdoc/>
        public Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            return _distributedCache.GetAsync(key, token);
        }

        ///<inheritdoc/>
        public void Refresh(string key)
        {
            _distributedCache.Refresh(key);
        }

        ///<inheritdoc/>
        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            return _distributedCache.RefreshAsync(key, token);
        }

        ///<inheritdoc/>
        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }

        ///<inheritdoc/>
        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            return _distributedCache.RemoveAsync(key, token);
        }

        ///<inheritdoc/>
        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            _distributedCache.Set(key, value, options);
        }

        ///<inheritdoc/>
        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            return _distributedCache.SetAsync(key, value, options, token);
        }

        ///<inheritdoc/>
        public T Get<T>(string key)
        {
            var response = _distributedCache.Get(key);
            return FromByteArray<T>(response);
        }

        ///<inheritdoc/>
        public async Task<T> GetAsync<T>(string key, CancellationToken token = default)
        {
            var response = await _distributedCache.GetAsync(key);
            return FromByteArray<T>(response);
        }

        ///<inheritdoc/>
        public void Set<T>(string key, T value, DistributedCacheEntryOptions options)
        {
            _distributedCache.Set(key, ToByteArray(value), options);
        }

        ///<inheritdoc/>
        public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            await _distributedCache.SetAsync(key, ToByteArray(value), options);
        }

        private byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
    }
}
