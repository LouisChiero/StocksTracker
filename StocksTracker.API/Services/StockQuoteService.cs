using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Framework.API.Quotes;

namespace StocksTracker.API.Services
{
    /// <summary>
    /// Implementation of <see cref="IStockQuoteService"/> interface, which queries stock quotes via a public API.
    /// </summary>
    public class StockQuoteService : IStockQuoteService
    {
        /// quote request URL format: http://finance.yahoo.com/d/quotes.csv?s=ticker1+ticker2+ticker3&f=sol1hgkjp

        private const string BaseApiUrl = "http://finance.yahoo.com/d/quotes.csv?s=";
        /*
         // s = sybmol
         // o = open
         // l1 = last trade price
         // h = day high
         // g = day low
         // k = year high
         // j = year low
         // p = previous close
         */
        private const string QuoteRequests = "&f=sol1hgkjp";

        /// <see cref="IStockQuoteService.GetStockQuotes"/>
        public async Task<IEnumerable<Quote>> GetStockQuotes(string[] tickerSymbols)
        {
            var rawQuotes = await GetQuotes(tickerSymbols);
            return rawQuotes.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(rawQuote => new Quote(rawQuote));
        }

        private static async Task<string> GetQuotes(string[] tickerSymbols)
        {
            using (var wc = new WebClient())
            {
                return
                    await
                        wc.DownloadStringTaskAsync(string.Format("{0}{1}{2}", BaseApiUrl,
                            string.Join("+", tickerSymbols), QuoteRequests));
            }
        }
    }
}