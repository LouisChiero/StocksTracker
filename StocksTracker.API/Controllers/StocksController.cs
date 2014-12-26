using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Framework.API.Stocks;

namespace StocksTracker.API.Controllers
{
    /// <summary>
    /// Controller provides methods for manipulating stocks using HTTP protocols.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Stocks")]
    public class StocksController : StocksTrackerBaseController
    {
        private readonly IStocks _stocksService;

        /// <summary>
        /// Instantiates the StocksController class.
        /// </summary>
        /// <param name="stocksService">Reference to an object that queries stocks.</param>
        public StocksController(IStocks stocksService)
        {
            _stocksService = stocksService;
        }

        // GET api/Stocks
        [HttpGet]
        [Route]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var stocks = await _stocksService.GetStocksAsync();
                return Ok(stocks.Select(record => MapStockRecordToObject(record)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/Stocks/1
        [HttpGet]
        [Route("{id:int}", Name = GetStockByIdRouteName)]
        public async Task<IHttpActionResult> Get(int id)
        {
            if (id == 0)
                return BadRequest();

            StockRecord stock;
            try
            {
                stock = await _stocksService.GetStockAsync(id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            if (stock == null)
                return NotFound();

            return Ok(MapStockRecordToObject(stock, GetStockByIdRouteName));
        }

        // GET api/Stocks/MSFT
        [HttpGet]
        [Route("{ticker}", Name = GetStockByTickerRouteName)]
        public async Task<IHttpActionResult> Get(string ticker)
        {
            StockRecord stock;
            try
            {
                stock = await _stocksService.GetStockAsync(ticker);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            if (stock == null)
                return NotFound();

            return Ok(MapStockRecordToObject(stock));
        }

        //[Route]
        //public IEnumerable<string> Get()
        //{
        //    var auth = Request.GetOwinContext().Authentication;

        //    var userId = User.Identity.GetUserId();
        //    var userName = User.Identity.GetUserName();
        //    var authType = User.Identity.AuthenticationType;
        //    var name = User.Identity.Name;
        //    return new string[] { "value1", "value2" };
        //}
    }
}

/*
     * LouisChiero/fitzwell
     * GC2_MqiuSryNoUVY7h7I2g_U7IsfeGf9KRKkw45uWs16d5vjieIjB4HGO_YI4mEzBQSbJgGAeHys7L74GaKqTxwvc4qvjJIDFEiGC1V24EcWA8QMWK6cx5P8vupdh4BNppvIjs9CkrOr-RmOTjNLpeidvshXCnd4vxx7D5L6p06pKNdnRUFHm0KhVhU8-oV7wJ3UB3CF6nx529n6GSPPF9j9F2Ga1r-YLSze4QbwuiOwXCY-R75YyZue-ACXq9B9FbUnGF8cftNFC_q2TQnp4F7WKbOk5rz721DoTNrOuRrrFCZ2velRxI-Q1a_gW_4N4tcWHDB1MUiKCdBe5uoC-ZlYUkPhlFuB4KP4B3uw9AT2nGOsaoNs3pAUWMKwH0RhEZU9PRUKa6Y3y8_0XseNFKmKBLI-Z7cBaCP5EO0m6NCh7hJVClz3ejepUBRoo1mAlIqgraAOi6GGsjm8XUm0vzMDbjCnW34MHPhJcPeBqz8DMspOLTrGdWGjaznwWsEOGzp3IqzPteW_k5xpMRy8vg
     */
