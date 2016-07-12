using System;
using System.Collections.Generic;
using System.Linq;
using Core.Framework.API.Data;
using Core.Framework.API.Stocks;
using System.Runtime.Caching;

namespace StocksTracker.API.Cache
{
    /// <summary>
    /// Class for managing cached <see cref="StockRecord"/> objects.
    /// </summary>
    public class StockCache : ICache<StockRecord>
    {
        private const string CacheName = "StocksTrackerCache";
        private readonly ObjectCache _theCache;

        public StockCache()
        {
            _theCache = MemoryCache.Default;
        }

        /// <see cref="ICache{T}.AddToCache"/>
        public void AddToCache(StockRecord cachedObject)
        {
            if (cachedObject == null)
                throw new ArgumentNullException("cachedObject");
            
            _theCache.Add(CreateCacheKey(cachedObject.StockRecordId), cachedObject, DateTimeOffset.MaxValue);
        }

        /// <see cref="ICache{T}.GetById"/>
        public StockRecord GetById(int id)
        {
            var key = CreateCacheKey(id);
            if (_theCache.Contains(key))
                return (StockRecord)_theCache.Get(key);
          
            return null;
        }

        /// <see cref="ICache{T}.GetAll"/>
        public IEnumerable<StockRecord> GetAll()
        {
            return _theCache.Select(pair => (StockRecord) pair.Value);
        }

        /// <see cref="ICache{T}.UpdateCachedObject"/>
        public void UpdateCachedObject(StockRecord updatedObject)
        {
            if (updatedObject == null)
                throw new ArgumentNullException("updatedObject");

            var key = CreateCacheKey(updatedObject.StockRecordId);
            if (_theCache.Contains(key))
                _theCache.Set(key, updatedObject, DateTimeOffset.MaxValue);
            else
                _theCache.Add(key, updatedObject, DateTimeOffset.MaxValue);
        }

        /// <see cref="ICache{T}.Clear"/>
        public void Clear()
        {
            _theCache.ToList().Clear();
        }

        private static string CreateCacheKey(int id)
        {
            return string.Format("{0}_{1}", CacheName, id);
        }
    }
}