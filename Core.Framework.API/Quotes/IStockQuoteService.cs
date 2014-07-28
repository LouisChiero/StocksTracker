using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Framework.API.Quotes
{
    /// <summary>
    /// Interface defines actions for stock quotes.
    /// </summary>
    public interface IStockQuoteService
    {
        /// <summary>
        /// Gets <see cref="Quote"/> objects for stocks.
        /// </summary>
        /// <param name="tickerSymbols">A collection of stock ticker symbols.</param>
        /// <returns>A collection of <see cref="Quote"/> objects for the given stocks.</returns>
        Task<IEnumerable<Quote>> GetStockQuotes(string[] tickerSymbols);
    }
}
