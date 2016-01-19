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
     * local:
     * GC2_MqiuSryNoUVY7h7I2g_U7IsfeGf9KRKkw45uWs16d5vjieIjB4HGO_YI4mEzBQSbJgGAeHys7L74GaKqTxwvc4qvjJIDFEiGC1V24EcWA8QMWK6cx5P8vupdh4BNppvIjs9CkrOr-RmOTjNLpeidvshXCnd4vxx7D5L6p06pKNdnRUFHm0KhVhU8-oV7wJ3UB3CF6nx529n6GSPPF9j9F2Ga1r-YLSze4QbwuiOwXCY-R75YyZue-ACXq9B9FbUnGF8cftNFC_q2TQnp4F7WKbOk5rz721DoTNrOuRrrFCZ2velRxI-Q1a_gW_4N4tcWHDB1MUiKCdBe5uoC-ZlYUkPhlFuB4KP4B3uw9AT2nGOsaoNs3pAUWMKwH0RhEZU9PRUKa6Y3y8_0XseNFKmKBLI-Z7cBaCP5EO0m6NCh7hJVClz3ejepUBRoo1mAlIqgraAOi6GGsjm8XUm0vzMDbjCnW34MHPhJcPeBqz8DMspOLTrGdWGjaznwWsEOGzp3IqzPteW_k5xpMRy8vg
     * 
     * api:
     * plW1fGADH8Pbl5OuNZm3cvKs57wzuz4Umwb-BhVuKK5LcZCqlgqGgCm-8ActS3OlRU1fyPT9aNIgxEebf5SL-ekI5LM_C4I1QIRXJxBWc4ZR_sLJKXRSwSjYpr7slMSZhZZstUa7wmCeK1GflHYJ4G_F6C_nLCbBiJWkNYqTElrt7745kS72fuyR9T7GSuo5oBE3ORd9EEaYZTOogZfjzlEgwkneic0ZNXHVR0mz20a93xYwQ6OqVLPYZyLSPVAKUSdXUaNaTbKc27R9m70m56P7f3nVuWI2TwrlOC5HAsRQzov-9dwiTkTiPH494c7rlOBrgjYmCkkK9hoGznIOnI7OIq76rPeHDoNBfat2mWjH3tEw9xweJXurzuSlKJ4XfFqdUru17IL93ad1n439T2pr9uZaxIVPSRMDfqUn9Gl_PGlpvmYuRPJ01QLiqYq8zrnt3hdymNOT8ObLLM-d7_cybjTgIioBDHT2MT_XW_FYHHzlB411rk3uNF94VHXUBtqPcZ8vtmdv1bOXPrTQxg
     */
