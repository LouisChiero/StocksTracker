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

        /// <summary>
        /// Instantiates the StockHeadlinesService class.
        /// </summary>
        /// <param name="stockCache">Reference to the stock cache object.</param>
        public StockHeadlinesService(ICache<StockRecord> stockCache)
        {
            _stockCache = stockCache;
        }

        /// <see cref="IStockHeadlinesService.GetStockHeadlinesAsync"/>
        public async Task<Headlines> GetStockHeadlinesAsync(int stockId)
        {
            var stock = _stockCache.GetById(stockId);
            if (stock == null)
                throw new ObjectNotFoundException();

            string url = string.Format("{0}{1}", "http://finance.yahoo.com/rss/headline?s=", stock.TickerSymbol);
            var serializer = new XmlSerializer(typeof(Core.Framework.API.Headlines.Xml.Headlines)); 
            
            string response;
            using (var wc = new WebClient())
                response = await wc.DownloadStringTaskAsync(url);

            if (!string.IsNullOrWhiteSpace(response))
            {
                var xmlString = new StringReader(response);
                var xmlReader = new XmlTextReader(xmlString);
                var result = (Core.Framework.API.Headlines.Xml.Headlines)serializer.Deserialize(xmlReader);

                if (result != null && result.Channel != null && result.Channel.Items != null &&
                    result.Channel.Items.Any())
                {
                    return new Headlines(stock.TickerSymbol, stock.Name,
                        result.Channel.Items.Select(
                            item => new NewsItem(item.Title, item.Link, item.Description, item.PubDate)).ToArray());
                }
            }

            return new Headlines(stock.TickerSymbol, stock.Name, new NewsItem[]{});
        }
    }
}