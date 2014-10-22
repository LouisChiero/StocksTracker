using System.Threading.Tasks;

namespace Core.Framework.API.Headlines
{
    /// <summary>
    /// Interface defines actions for stock headlines.
    /// </summary>
    public interface IStockHeadlinesService
    {
        /// <summary>
        /// Asynchronously gets <see cref="Headlines"/>s for a given stock.
        /// </summary>
        /// <param name="stockId">The stock ID.</param>
        /// <returns>A <see cref="Headlines"/> object.</returns>
        Task<Headlines> GetStockHeadlinesAsync(int stockId);

        /// <summary>
        /// Asynchronously gets <see cref="Headlines"/>s for a given stock.
        /// </summary>
        /// <param name="tickerSymbol">The stock ticker symbol.</param>
        /// <returns>A <see cref="Headlines"/> object.</returns>
        Task<Headlines> GetStockHeadlinesAsync(string tickerSymbol);
    }
}
