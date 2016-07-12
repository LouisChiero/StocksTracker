using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Core.Framework.API.Data;
using Core.Framework.API.Stocks;
using Core.Framework.API.Support;
using Data.Context;
using StocksTracker.API.Extensions;

namespace StocksTracker.API.Cache
{
    /// <summary>
    /// Class that manages the stock cache.
    /// </summary>
    public class StockCacheManager : ICacheManager<StockRecord>
    {
        private readonly IObjectFactory<StocksTrackerContext> _stocksTrackerContextFactory;
        private readonly ICache<StockRecord> _stockCache;

        static readonly object LockObject = new object();

        /// <summary>
        /// Instantiates the StockCacheManager class.
        /// </summary>
        /// <param name="stocksTrackerContextFactory">Reference to a factory that provides <see cref="StocksTrackerContext"/> objects.</param>
        /// <param name="stockCache">Reference to the stock cache object.</param>
        public StockCacheManager(
            IObjectFactory<StocksTrackerContext> stocksTrackerContextFactory, 
            ICache<StockRecord> stockCache)
        {
            _stocksTrackerContextFactory = stocksTrackerContextFactory;
            _stockCache = stockCache;
        }

        /// <see cref="ICacheManager{T}.InitializeCache"/>
        public void InitializeCache()
        {
            lock (LockObject)
            {
                _stockCache.Clear();

                using (var ctx = _stocksTrackerContextFactory.GetObject())
                {
                    foreach (var stock in ctx.Stocks.AsNoTracking().ToList())
                        _stockCache.AddToCache(stock.MapStockRecord());
                }
            }
        }

        /// <see cref="ICacheManager{T}.AddObjectToCache"/>
        public void AddObjectToCache(StockRecord cacheObject)
        {
            lock (LockObject)
                _stockCache.AddToCache(cacheObject);
        }

        /// <see cref="ICacheManager{T}.GetCachedObject"/>
        public StockRecord GetCachedObject(int id)
        {
            return _stockCache.GetById(id);
        }

        /// <see cref="ICacheManager{T}.GetAllCachedObjects"/>
        public IEnumerable<StockRecord> GetAllCachedObjects()
        {
            return _stockCache.GetAll();
        }

        /// <see cref="ICacheManager{T}.UpdateCachedObject"/>
        public void UpdateCachedObject(StockRecord cachedObject)
        {
            lock (LockObject)
                _stockCache.UpdateCachedObject(cachedObject);
        }

        /// <see cref="ICacheManager{T}.ClearCachedObjects"/>
        public void ClearCachedObjects()
        {
            lock (LockObject)
                _stockCache.Clear();
        }

        /// <see cref="ICacheManager{T}.ObjectExistsInCache"/>
        public bool ObjectExistsInCache(int id)
        {
            return _stockCache.GetById(id) != null;
        }
    }
}