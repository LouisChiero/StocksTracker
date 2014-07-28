using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Core.Framework.API.Data;
using Core.Framework.API.Stocks;

namespace StocksTracker.API.Cache
{
    /// <summary>
    /// Class for managing cached <see cref="StockRecord"/> objects.
    /// </summary>
    public class StockCache : ICache<StockRecord>
    {
        private readonly ConcurrentDictionary<int, StockRecord> _cache = new ConcurrentDictionary<int, StockRecord>();

        /// <see cref="ICache{T}.AddToCache"/>
        public void AddToCache(StockRecord cachedObject)
        {
            if (cachedObject == null)
                throw new ArgumentNullException("cachedObject");

            _cache.TryAdd(cachedObject.StockRecordId, cachedObject);
        }

        /// <see cref="ICache{T}.GetById"/>
        public StockRecord GetById(int id)
        {
            StockRecord stock = default(StockRecord);
            if (_cache.ContainsKey(id))
                _cache.TryGetValue(id, out stock);

            return stock;
        }

        /// <see cref="ICache{T}.GetAll"/>
        public IEnumerable<StockRecord> GetAll()
        {
            return _cache.Values;
        }

        /// <see cref="ICache{T}.UpdateCachedObject"/>
        public void UpdateCachedObject(StockRecord updatedObject)
        {
            if (updatedObject == null)
                throw new ArgumentNullException("updatedObject");

            StockRecord existingStock;
            if (_cache.TryGetValue(updatedObject.StockRecordId, out existingStock))
            {
                _cache.TryUpdate(updatedObject.StockRecordId, updatedObject, existingStock);
            }
        }

        /// <see cref="ICache{T}.Clear"/>
        public void Clear()
        {
            _cache.Clear();
        }
    }
}