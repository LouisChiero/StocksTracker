using System;
using System.Web.Http;
using Core.Framework.API.Charts;

namespace StocksTracker.API.Controllers
{
    /// <summary>
    /// Controller provides methods for querying stock charts using HTTP protocols.
    /// </summary>
    [Authorize]
    public class ChartsController : StocksTrackerBaseController
    {
        private readonly IStockChartsService _stockChartsService;

        /// <summary>
        /// Instantiates the ChartsController class.
        /// </summary>
        /// <param name="stockChartsService">Reference to an object that provides stock chart services.</param>
        public ChartsController(IStockChartsService stockChartsService)
        {
            _stockChartsService = stockChartsService;
        }

        // GET api/Stocks/1/Charts?timeSpan=OneYear&chartType=Bar&chartSize=Middle
        [HttpGet]
        [Route("api/Stocks/{stockId:int}/Charts")]
        public IHttpActionResult Get(int stockId, 
            StockChartTimeSpan timeSpan = StockChartTimeSpan.OneYear, 
            StockChartType chartType = StockChartType.Line, 
            StockChartSize chartSize = StockChartSize.Large)
        {
            if (stockId == 0)
                BadRequest();

            Uri chartUri;
            try
            {
                chartUri =
                    _stockChartsService.GetStockChartResource(new StockChartQueryParameters(timeSpan, chartType,
                        chartSize, stockId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(new {url = chartUri.ToString()});
        }

        // GET api/Stocks/AAPL/Charts?timeSpan=OneYear&chartType=Bar&chartSize=Middle
        [HttpGet]
        [Route("api/Stocks/{ticker}/Charts")]
        public IHttpActionResult Get(string ticker,
            StockChartTimeSpan timeSpan = StockChartTimeSpan.OneYear,
            StockChartType chartType = StockChartType.Line,
            StockChartSize chartSize = StockChartSize.Large)
        {
            Uri chartUri;
            try
            {
                chartUri =
                    _stockChartsService.GetStockChartResource(new StockChartQueryParameters(timeSpan, chartType,
                        chartSize, tickerSymbol: ticker));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(new { url = chartUri.ToString() });
        }
    }
}
