using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Framework.API.Data;
using Core.Framework.API.Stocks;
using StocksTracker.API.Extensions;

namespace StocksTracker.API.Services
{
    /// <summary>
    /// Implementation of <see cref="IStocks"/> interface, which queries stocks.
    /// </summary>
    public class StocksService : IStocks
    {
        private readonly ICache<StockRecord> _stockCache;
        private readonly IStockUpdater _stockUpdater;

        /// <summary>
        /// Instantiates the StocksService class.
        /// </summary>
        /// <param name="stockCache">Reference to the stock cache object.</param>
        /// <param name="stockUpdater">Reference to the stock updater object.</param>
        public StocksService(
            ICache<StockRecord> stockCache,
            IStockUpdater stockUpdater)
        {
            _stockCache = stockCache;
            _stockUpdater = stockUpdater;
        }

        /// <see cref="IStocks.GetStockAsync"/>
        public async Task<StockRecord> GetStockAsync(int stockId)
        {
            var stock = await Task.Run(() => _stockCache.GetById(stockId));
            await _stockUpdater.UpdateStocksQuoteDataAsync(stock.ToEnumerable());

            return await Task.Run(() => _stockCache.GetById(stockId));
        }

        /// <see cref="IStocks.GetStocksAsync"/>
        public async Task<IEnumerable<StockRecord>> GetStocksAsync()
        {
            return await Task.Run(() => _stockCache.GetAll());
        }
    }
}