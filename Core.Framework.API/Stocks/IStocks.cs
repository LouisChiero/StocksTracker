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
        /// Asynchronously queries a stock for the given ticker symbol.
        /// </summary>
        /// <param name="tickerSymbol">The stock ticker symbol.</param>
        /// <returns>A <see cref="StockRecord"/> object, or null if not found.</returns>
        Task<StockRecord> GetStockAsync(string tickerSymbol);

        /// <summary>
        /// Asynchronously queries all stocks.
        /// </summary>
        /// <returns>A collection of <see cref="StockRecord"/> objects.</returns>
        Task<IEnumerable<StockRecord>> GetStocksAsync();

        /// <summary>
        /// Asynchronously queries stocks based on a search term.
        /// </summary>
        /// <param name="term">The search term for querying stocks.</param>
        /// <returns>A collection of search values.</returns>
        Task<IEnumerable<dynamic>> GetStocksSearchValuesAsync(string term);
    }
}
