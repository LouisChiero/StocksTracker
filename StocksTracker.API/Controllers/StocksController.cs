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
     * n2V1JQYrf5RTFiswsaEIvM7ZHI3f6ZZ2Jismuep2M-KM4tn_goemJOtaCDQCiEIE-rHsYwqi1RB8nyV-uCWExWvpLih0c1zSSk5xE6K9qPr-eKhMR8lX0SdlhDk5Z8hk9I0Szwtq6fIhxJHurLPRrN3y-aT8DteIJcHBmPvghiSbt46nb_-bGYaNNDyHK1KszVSUsl23a_TkMeQpjB1cIcNPdR7q3MRzDYp-pYGkN2Ahevn4gCMMceqgLN-gC-e2sSVcHp1U6g9iXmU6euODgQNu7f3jcuPHr9OpmiOmd49H3nrskz3KsdEjT-bdTgW-GwB4R-ubW5HpvWLaML-9aBGTh0YBoyEVSTdPbDEstl1FO7CI80qmg2sTdZ7SE-2M6AdRUkuHlpXkqOX1_jJxSeeGPI2aIaeMshPsaiznqEgYdALm2uFhYssZ8WosbMJNlXD7R7u9k181YnlorPRLkU-D5XCsWQxrCVx47lf409YAn3pk8wyp2nHRmiWe9gb-lUAHhcEH0xU4eQsNywqt_w
     * 
     * api:
     * plW1fGADH8Pbl5OuNZm3cvKs57wzuz4Umwb-BhVuKK5LcZCqlgqGgCm-8ActS3OlRU1fyPT9aNIgxEebf5SL-ekI5LM_C4I1QIRXJxBWc4ZR_sLJKXRSwSjYpr7slMSZhZZstUa7wmCeK1GflHYJ4G_F6C_nLCbBiJWkNYqTElrt7745kS72fuyR9T7GSuo5oBE3ORd9EEaYZTOogZfjzlEgwkneic0ZNXHVR0mz20a93xYwQ6OqVLPYZyLSPVAKUSdXUaNaTbKc27R9m70m56P7f3nVuWI2TwrlOC5HAsRQzov-9dwiTkTiPH494c7rlOBrgjYmCkkK9hoGznIOnI7OIq76rPeHDoNBfat2mWjH3tEw9xweJXurzuSlKJ4XfFqdUru17IL93ad1n439T2pr9uZaxIVPSRMDfqUn9Gl_PGlpvmYuRPJ01QLiqYq8zrnt3hdymNOT8ObLLM-d7_cybjTgIioBDHT2MT_XW_FYHHzlB411rk3uNF94VHXUBtqPcZ8vtmdv1bOXPrTQxg
     */
