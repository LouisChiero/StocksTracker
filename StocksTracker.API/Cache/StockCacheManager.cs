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
    public class StockCacheManager : ICacheManager
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

        /// <see cref="ICacheManager.LoadDataCache"/>
        public void LoadDataCache()
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
    }
}