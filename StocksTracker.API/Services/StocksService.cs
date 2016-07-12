using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ICacheManager<StockRecord> _cacheManager;
        private readonly IStockUpdater _stockUpdater;

        /// <summary>
        /// Instantiates the StocksService class.
        /// </summary>
        /// <param name="cacheManager">Reference to the stock cache object.</param>
        /// <param name="stockUpdater">Reference to the stock updater object.</param>
        public StocksService(
            ICacheManager<StockRecord> cacheManager,
            IStockUpdater stockUpdater)
        {
            _cacheManager = cacheManager;
            _stockUpdater = stockUpdater;
        }

        /// <see cref="IStocks.GetStockAsync(int)"/>
        public async Task<StockRecord> GetStockAsync(int stockId)
        {
            var stock = await Task.Run(() => _cacheManager.GetCachedObject(stockId));
            await _stockUpdater.UpdateStocksQuoteDataAsync(stock.ToEnumerable());

            return await Task.Run(() => _cacheManager.GetCachedObject(stockId));
        }

        /// <see cref="IStocks.GetStockAsync(string)"/>
        public async Task<StockRecord> GetStockAsync(string tickerSymbol)
        {
            if (string.IsNullOrWhiteSpace(tickerSymbol))
                return null;

            var allStocks = await Task.Run(() => _cacheManager.GetAllCachedObjects());
            var theStock =
                allStocks.SingleOrDefault(
                    record =>
                        String.Equals(record.TickerSymbol, tickerSymbol, StringComparison.InvariantCultureIgnoreCase));

            if (theStock == null) 
                return null;

            await _stockUpdater.UpdateStocksQuoteDataAsync(theStock.ToEnumerable());
            return await Task.Run(() => _cacheManager.GetCachedObject(theStock.StockRecordId));
        }

        /// <see cref="IStocks.GetStocksAsync"/>
        public async Task<IEnumerable<StockRecord>> GetStocksAsync()
        {
            return await Task.Run(() => _cacheManager.GetAllCachedObjects());
        }

        /// <see cref="IStocks.GetStocksAsync"/>
        public async Task<IEnumerable<dynamic>> GetStocksSearchValuesAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Enumerable.Empty<string>();

            // use lower case for comparison(s)
            term = term.ToLowerInvariant();

            var getAll = await Task.Run(() => _cacheManager.GetAllCachedObjects());
            var allStocks = getAll.ToList();

            // give preference to ticker matches
            var tickerMatches =
                allStocks.Where(record => record.TickerSymbol.ToLowerInvariant().Contains(term)).ToList();
            var nameMatches = allStocks.Where(record => record.Name.ToLowerInvariant().Contains(term)).ToList();

            // return matches in format: id = 1, ticker = MSFT, name = Microsoft Corporation
            return await Task.Run( () => tickerMatches
                .Concat(nameMatches)
                .Distinct()
                .Select(record => new
                {
                    id = record.StockRecordId,
                    ticker = record.TickerSymbol,
                    name = record.Name
                }));
        }
    }
}