using System;
using Core.Framework.API.Stocks;
using Data.Models;
using StocksTracker.API.UserAdministration;

namespace StocksTracker.API.Extensions
{
    /// <summary>
    /// Class provides extension methods on domain and view model objects.
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// Maps a <see cref="Stock"/> domain object to a <see cref="StockRecord"/> property bag object.
        /// </summary>
        /// <param name="stock">The <see cref="Stock"/> to map.</param>
        /// <returns>The mapped <see cref="StockRecord"/> object.</returns>
        public static StockRecord MapStockRecord(this Stock stock)
        {
            if (stock == null)
                throw new ArgumentNullException("stock");

            return new StockRecord
            {
                DayHigh = stock.DayHigh,
                DayLow = stock.DayLow,
                LastTrade = stock.LastTrade,
                LastUpdatedDateTimeUniversal = stock.LastUpdatedDateTimeUniversal,
                Name = stock.Name,
                OpenPrice = stock.OpenPrice,
                PreviousClose = stock.PreviousClose,
                StockRecordId = stock.StockId,
                TickerSymbol = stock.TickerSymbol,
                YearHigh = stock.YearHigh,
                YearLow = stock.YearLow
            };
        }

        /// <summary>
        /// Maps a <see cref="StockTracker"/> domain object to a <see cref="StockTrackerRecord"/> property bag object.
        /// </summary>
        /// <param name="stockTracker">The <see cref="StockTracker"/> to map.</param>
        /// <returns>The mapped <see cref="StockTrackerRecord"/> object.</returns>
        public static StockTrackerRecord MapStockTrackerRecord(this StockTracker stockTracker)
        {
            if (stockTracker == null)
                throw new ArgumentNullException("stockTracker");

            return new StockTrackerRecord
            {
                IsDefault = stockTracker.IsDefault,
                Name = stockTracker.Name,
                StockTrackerRecordId = stockTracker.StockTrackerId,
                Stocks = new StockRecord[] {}
            };
        }

        /// <summary>
        /// Maps an <see cref="ApplicationUser"/> domain object to a <see cref="User"/> object.
        /// </summary>
        /// <param name="user">The <see cref="ApplicationUser"/> to map.</param>
        /// <returns>The mapped <see cref="User"/> object.</returns>
        public static User MapUserRecord(this ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return new User
            {
                FirstName = user.FirstName, 
                Identity = user, 
                LastName = user.LastName
            };
        }

        /// <summary>
        /// Provides a custom method on <see cref="StockRecord.LastUpdatedDateTimeUniversal"/>, testing the last time a <see cref="StockRecord"/> was updated.
        /// </summary>
        /// <param name="stock">The <see cref="StockRecord"/>.</param>
        /// <param name="intervalMinutes">The time interval to check against the <see cref="StockRecord.LastUpdatedDateTimeUniversal"/> field.</param>
        /// <returns>True if the <see cref="StockRecord"/> needs updating, or false if not.</returns>
        public static bool StockNeedsUpdate(this StockRecord stock, int intervalMinutes = 15)
        {
            return !stock.LastUpdatedDateTimeUniversal.HasValue
                   || stock.LastUpdatedDateTimeUniversal.Value < DateTimeOffset.UtcNow.AddMinutes(-intervalMinutes);
        }
    }
}