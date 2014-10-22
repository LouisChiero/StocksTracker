using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Framework.API.Headlines;

namespace StocksTracker.API.Controllers
{
    /// <summary>
    /// Controller provides methods for querying stock headlines using HTTP protocols.
    /// </summary>
    [Authorize]
    public class HeadlinesController : StocksTrackerBaseController
    {
        private readonly IStockHeadlinesService _stockHeadlinesService;

        /// <summary>
        /// Instantiates the HeadlinesController class.
        /// </summary>
        /// <param name="stockHeadlinesService">Reference to an object that provides stock headline services.</param>
        public HeadlinesController(IStockHeadlinesService stockHeadlinesService)
        {
            _stockHeadlinesService = stockHeadlinesService;
        }

        // GET api/Stocks/1/Headlines
        [HttpGet]
        [Route("api/Stocks/{stockId:int}/Headlines")] 
        public async Task<IHttpActionResult> Get(int stockId)
        {
            if (stockId == 0)
                BadRequest();

            Headlines headlines;
            try
            {
                headlines = await _stockHeadlinesService.GetStockHeadlinesAsync(stockId);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(MapHeadlinesToObject(headlines));
        }

        // GET api/Stocks/NFLX/Headlines
        [HttpGet]
        [Route("api/Stocks/{ticker}/Headlines")]
        public async Task<IHttpActionResult> Get(string ticker)
        {
            Headlines headlines;
            try
            {
                headlines = await _stockHeadlinesService.GetStockHeadlinesAsync(ticker);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(MapHeadlinesToObject(headlines));
        }

        private static object MapHeadlinesToObject(Headlines headlines)
        {
            return new
            {
                tickerSymbol = headlines.TickerSymbol,
                stockName = headlines.StockName,
                newsItems = headlines.NewsItems.Select(item => new {title = item.Title, link = item.Link, description = item.Description, pubDate = item.PubDate}).ToArray()
            };
        }
    }
}
