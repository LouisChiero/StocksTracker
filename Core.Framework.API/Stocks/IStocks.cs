using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Framework.API.Stocks
{
    /// <summary>
    /// Interface defines actions that can be performed on stock objects.
    /// </summary>
    public interface IStocks
    {
        /// <summary>
        /// Asynchronously queries a stock for the given ID.
        /// </summary>
        /// <param name="stockId">The stock ID.</param>
        /// <returns>A <see cref="StockRecord"/> object, or null if not found.</returns>
        Task<StockRecord> GetStockAsync(int stockId);

        /// <summary>
        /// Asynchronously queries all stocks.
        /// </summary>
        /// <returns>A collection of <see cref="StockRecord"/> objects.</returns>
        Task<IEnumerable<StockRecord>> GetStocksAsync();
    }
}
