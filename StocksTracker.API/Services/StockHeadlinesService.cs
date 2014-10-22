using System;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Core.Framework.API.Data;
using Core.Framework.API.Headlines;
using Core.Framework.API.Stocks;

namespace StocksTracker.API.Services
{
    /// <summary>
    /// Implementation of <see cref="IStockHeadlinesService"/> interface, which queries stock headlines via a public API.
    /// </summary>
    public class StockHeadlinesService : IStockHeadlinesService
    {
        private readonly ICache<StockRecord> _stockCache;
        private const string HeadlinesBaseUrl = "http://finance.yahoo.com/rss/headline?s=";

        /// <summary>
        /// Instantiates the StockHeadlinesService class.
        /// </summary>
        /// <param name="stockCache">Reference to the stock cache object.</param>
        public StockHeadlinesService(ICache<StockRecord> stockCache)
        {
            _stockCache = stockCache;
        }

        /// <see cref="IStockHeadlinesService.GetStockHeadlinesAsync(int)"/>
        public async Task<Headlines> GetStockHeadlinesAsync(int stockId)
        {
            var stock = _stockCache.GetById(stockId);
            if (stock == null)
                throw new ObjectNotFoundException();

            string url = string.Format("{0}{1}", HeadlinesBaseUrl, stock.TickerSymbol);
            string response;
            using (var wc = new WebClient())
                response = await wc.DownloadStringTaskAsync(url);

            return ProcessHeadlines(stock, response);
        }

        /// <see cref="IStockHeadlinesService.GetStockHeadlinesAsync(string)"/>
        public async Task<Headlines> GetStockHeadlinesAsync(string tickerSymbol)
        {
            var allStocks = await Task.Run(() => _stockCache.GetAll());
            var theStock =
                allStocks.SingleOrDefault(
                    record =>
                        String.Equals(record.TickerSymbol, tickerSymbol, StringComparison.InvariantCultureIgnoreCase));

            if (theStock == null)
                throw new ObjectNotFoundException();

            string url = string.Format("{0}{1}", HeadlinesBaseUrl, theStock.TickerSymbol);
            string response;
            using (var wc = new WebClient())
                response = await wc.DownloadStringTaskAsync(url);

            return ProcessHeadlines(theStock, response);
        }

        private static Headlines ProcessHeadlines(StockRecord stock, string rawHeadlines)
        {
            if (!string.IsNullOrWhiteSpace(rawHeadlines))
            {
                var xmlString = new StringReader(rawHeadlines);
                var xmlReader = new XmlTextReader(xmlString);
                var serializer = new XmlSerializer(typeof(Core.Framework.API.Headlines.Xml.Headlines)); 
                var result = (Core.Framework.API.Headlines.Xml.Headlines)serializer.Deserialize(xmlReader);

                if (result != null && result.Channel != null && result.Channel.Items != null &&
                    result.Channel.Items.Any())
                {
                    return new Headlines(stock.TickerSymbol, stock.Name,
                        result.Channel.Items.Select(
                            item => new NewsItem(item.Title, item.Link, item.Description, item.PubDate)).ToArray());
                }
            }

            return new Headlines(stock.TickerSymbol, stock.Name, new NewsItem[] { });
        }
    }
}