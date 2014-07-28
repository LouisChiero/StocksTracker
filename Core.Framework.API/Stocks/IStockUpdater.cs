using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Framework.API.Stocks
{
    /// <summary>
    /// Interface defines methods for updating stocks.
    /// </summary>
    public interface IStockUpdater
    {
        /// <summary>
        /// Asynchronously updates a collection of stocks with quote data.
        /// </summary>
        /// <param name="stocks">A collection of stocks to update.</param>
        /// <returns>The result of the update action.</returns>
        Task<StockTrackerOperationResult> UpdateStocksQuoteDataAsync(IEnumerable<StockRecord> stocks);
    }
}
