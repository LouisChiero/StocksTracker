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
            StockChartTimeSpan timeSpan = StockChartTimeSpan.OneDay, 
            StockChartType chartType = StockChartType.Line, 
            StockChartSize chartSize = StockChartSize.Small)
        {
            if (stockId == 0)
                BadRequest();

            Uri chartUri;
            try
            {
                chartUri =
                    _stockChartsService.GetStockChartResource(new StockChartQueryParameters(stockId, timeSpan, chartType,
                        chartSize));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(new {url = chartUri.ToString()});
        }
    }
}
