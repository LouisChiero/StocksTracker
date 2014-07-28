using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Framework.API.Data;
using Core.Framework.API.Quotes;
using Core.Framework.API.Stocks;
using Core.Framework.API.Support;
using Data.Context;
using Data.Models;
using StocksTracker.API.Extensions;

namespace StocksTracker.API.Services
{
    /// <summary>
    /// Implementation of <see cref="IStockUpdater"/> interface, which provides updates for stocks data that must be queried outside of 
    /// the Stocks Tracker domain.
    /// </summary>
    public class StockUpdater : IStockUpdater
    {
        private readonly IStockQuoteService _stockQuoteService;
        private readonly IObjectFactory<StocksTrackerContext> _stocksTrackerContextFactory;
        private readonly ICache<StockRecord> _stockCache;

        static readonly object LockObject = new object();

        /// <summary>
        /// Instantiates the StockUpdater class.
        /// </summary>
        /// <param name="stockQuoteService">Reference to an object that provides stock quotes.</param>
        /// <param name="stocksTrackerContextFactory">Reference to a factory that provides <see cref="StocksTrackerContext"/> objects.</param>
        /// <param name="stockCache">Reference to the stock cache object.</param>
        public StockUpdater(
            IStockQuoteService stockQuoteService,
            IObjectFactory<StocksTrackerContext> stocksTrackerContextFactory,
            ICache<StockRecord> stockCache)
        {
            _stockQuoteService = stockQuoteService;
            _stocksTrackerContextFactory = stocksTrackerContextFactory;
            _stockCache = stockCache;
        }

        /// <see cref="IStockUpdater.UpdateStocksQuoteDataAsync"/>
        public async Task<StockTrackerOperationResult> UpdateStocksQuoteDataAsync(IEnumerable<StockRecord> stocks)
        {
            var stocksNeedingUpdate = stocks.Where(record => record.StockNeedsUpdate()).ToList();
            var quotesAsync = await _stockQuoteService.GetStockQuotes(
                stocksNeedingUpdate
                .Select(record => record.TickerSymbol)
                .ToArray());

            var quotes = quotesAsync as Quote[] ?? quotesAsync.ToArray();
            lock (LockObject)
            {
                using (var ctx = _stocksTrackerContextFactory.GetObject())
                {
                    foreach (var stock in stocksNeedingUpdate)
                    {
                        var updateStock = ctx.Stocks.Find(stock.StockRecordId);
                        UpdateStockFromQuote(updateStock, quotes.First(q => q.Ticker == updateStock.TickerSymbol));
                        ctx.SaveChanges();
                        _stockCache.UpdateCachedObject(updateStock.MapStockRecord());
                    }
                }
            }

            return StockTrackerOperationResult.UpdateSucceeded;
        }

        private static void UpdateStockFromQuote(Stock stock, Quote quote)
        {
            if (stock == null)
                return;

            if (quote.Open.HasValue)
                stock.OpenPrice = quote.Open.Value;
            if (quote.LastTrade.HasValue)
                stock.LastTrade = quote.LastTrade.Value;
            if (quote.DayHigh.HasValue)
                stock.DayHigh = quote.DayHigh.Value;
            if (quote.DayLow.HasValue)
                stock.DayLow = quote.DayLow.Value;
            if (quote.YearHigh.HasValue)
                stock.YearHigh = quote.YearHigh.Value;
            if (quote.YearLow.HasValue)
                stock.YearLow = quote.YearLow.Value;
            if (quote.PreviousClose.HasValue)
                stock.PreviousClose = quote.PreviousClose.Value;

            stock.LastUpdatedDateTimeUniversal = DateTimeOffset.UtcNow;
        }
    }
}