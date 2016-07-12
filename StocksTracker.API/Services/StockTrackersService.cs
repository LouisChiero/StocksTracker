using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using Core.Framework.API.Data;
using Core.Framework.API.Stocks;
using Core.Framework.API.Support;
using Data.Context;
using Data.Models;
using StocksTracker.API.Extensions;

namespace StocksTracker.API.Services
{
    /// <summary>
    /// Implementation of <see cref="IStockTrackers"/> interface, which queries stocks.
    /// </summary>
    public class StockTrackersService : IStockTrackers
    {
        private readonly IObjectFactory<StocksTrackerContext> _stocksTrackerContextFactory;
        private readonly ICacheManager<StockRecord> _cacheManager;
        private readonly IStockUpdater _stockUpdater;

        /// <summary>
        /// Instantiates the StockTrackersService class.
        /// </summary>
        /// <param name="stocksTrackerContextFactory">Reference to a factory that provides <see cref="StocksTrackerContext"/> objects.</param>
        /// <param name="cacheManager">Reference to the stock cache object.</param>
        /// <param name="stockUpdater">Reference to an object that can update stocks.</param>
        public StockTrackersService(
            IObjectFactory<StocksTrackerContext> stocksTrackerContextFactory,
            ICacheManager<StockRecord> cacheManager,
            IStockUpdater stockUpdater)
        {
            _stocksTrackerContextFactory = stocksTrackerContextFactory;
            _cacheManager = cacheManager;
            _stockUpdater = stockUpdater;
        }

        /// <see cref="IStockTrackers.GetStockTrackerAsync"/>
        public async Task<StockTrackerRecord> GetStockTrackerAsync(int stockTrackerId)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                var stockTracker = await ctx.StockTrackers
                    .AsNoTracking()
                    .Include(tracker => tracker.StockTrackerStocks)
                    .SingleOrDefaultAsync(tracker => tracker.StockTrackerId == stockTrackerId);

                if (stockTracker == null)
                    throw new ObjectNotFoundException();

                // update stocks
                var stockTrackerStocksIds = stockTracker.StockTrackerStocks.Select(stock => stock.StockId).ToList();
                var updateOperationResult = await
                    _stockUpdater.UpdateStocksQuoteDataAsync(stockTrackerStocksIds.Distinct().Select(i => _cacheManager.GetCachedObject(i)));
                
                // return
                var returnObject = stockTracker.MapStockTrackerRecord();
                returnObject.Stocks = stockTrackerStocksIds.Select(i => _cacheManager.GetCachedObject(i)).ToArray();

                return returnObject;
            }
        }

        /// <see cref="IStockTrackers.GetStockTrackersForUser"/>
        public async Task<IEnumerable<StockTrackerRecord>> GetStockTrackersForUser(string userId)
        {
            var returnStockTrackers = new List<StockTrackerRecord>();
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                var stockTrackers = await ctx.StockTrackers
                    .AsNoTracking()
                    .Where(tracker => tracker.ApplicationUserId == userId)
                    .Include(tracker => tracker.StockTrackerStocks)
                    .ToListAsync();

                // key: stock tracker ID; value: collection of stock IDs
                var stockTrackersDictionary = stockTrackers.ToDictionary(tracker => tracker.StockTrackerId,
                    tracker => tracker.StockTrackerStocks.Select(stock => stock.StockId));
                
                // update stocks
                var updateOperationResult =
                    await _stockUpdater.UpdateStocksQuoteDataAsync(
                            stockTrackersDictionary.Values.SelectMany(
                                ints => ints.Distinct().Select(i => _cacheManager.GetCachedObject(i))));

                // return
                stockTrackers.ForEach(tracker =>
                    {
                        var str = tracker.MapStockTrackerRecord();
                        str.Stocks = stockTrackersDictionary[tracker.StockTrackerId].Select(i => _cacheManager.GetCachedObject(i)).ToArray();
                        returnStockTrackers.Add(str);
                    });
            }

            return returnStockTrackers;
        }

        /// <see cref="IStockTrackers.AddStockTrackerForUserAsync"/>
        public async Task<StockTrackerRecord> AddStockTrackerForUserAsync(StockTrackerRecord addStockTracker, string userId)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                var stockTracker = new StockTracker
                {
                    ApplicationUserId = userId,
                    Name = addStockTracker.Name,
                    IsDefault = addStockTracker.IsDefault
                };

                ctx.StockTrackers.Add(stockTracker);
                await ctx.SaveChangesAsync();
                
                // return
                return stockTracker.MapStockTrackerRecord();
            }
        }

        /// <see cref="IStockTrackers.RemoveStockTrackerAsync"/>
        public async Task<StockTrackerOperationResult> RemoveStockTrackerAsync(int stockTrackerId)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                var stockTracker =
                    await ctx.StockTrackers.SingleOrDefaultAsync(tracker => tracker.StockTrackerId == stockTrackerId);
                if (stockTracker == null)
                    return StockTrackerOperationResult.RemoveSucceeded;

                stockTracker.StockTrackerStocks.ToList().ForEach(stock => ctx.StockTrackerStocks.Remove(stock));
                ctx.StockTrackers.Remove(stockTracker);

                await ctx.SaveChangesAsync();

                return StockTrackerOperationResult.RemoveSucceeded;
            }
        }

        /// <see cref="IStockTrackers.UpdateStockTrackerAsync"/>
        public async Task<StockTrackerRecord> UpdateStockTrackerAsync(StockTrackerRecord updateStockTracker)
        {
            using (var ctx = _stocksTrackerContextFactory.GetObject())
            {
                var stockTracker =
                    await
                        ctx.StockTrackers.Include(tracker => tracker.StockTrackerStocks)
                            .SingleOrDefaultAsync(
                                tracker => tracker.StockTrackerId == updateStockTracker.StockTrackerRecordId);

                if (stockTracker == null)
                    throw new ObjectNotFoundException();

                if (!string.IsNullOrWhiteSpace(updateStockTracker.Name)
                    && updateStockTracker.Name != stockTracker.Name.Trim())
                {
                    stockTracker.Name = updateStockTracker.Name;
                }

                if (updateStockTracker.IsDefault != stockTracker.IsDefault)
                    stockTracker.IsDefault = updateStockTracker.IsDefault;

                // add/remove stocks from tracker
                var newStockIds = updateStockTracker.Stocks.Select(record => record.StockRecordId).ToList();
                var currentStockTrackerStockIds =
                    stockTracker.StockTrackerStocks.Select(stock => stock.StockId).ToList();

                var stocksToRemove = currentStockTrackerStockIds.Except(newStockIds).ToList();
                var stocksToAdd = newStockIds.Except(currentStockTrackerStockIds).ToList();

                stocksToRemove.ForEach(i =>
                    {
                        var stockTrackerStock =
                            ctx.StockTrackerStocks.SingleOrDefaultAsync(
                                stock =>
                                    stock.StockId == i &&
                                    stock.StockTrackerId == stockTracker.StockTrackerId).Result;

                        if (stockTrackerStock != null)
                            ctx.StockTrackerStocks.Remove(stockTrackerStock);
                    });

                stocksToAdd.ForEach(i => ctx.StockTrackerStocks.Add(new StockTrackerStock
                {
                    StockId = i,
                    StockTrackerId = stockTracker.StockTrackerId
                }));

                await ctx.SaveChangesAsync();

                var returnObject = stockTracker.MapStockTrackerRecord();
                returnObject.Stocks =
                    stockTracker.StockTrackerStocks.Select(stock => _cacheManager.GetCachedObject(stock.StockId)).ToArray();

                return returnObject;
            }
        }
    }
}