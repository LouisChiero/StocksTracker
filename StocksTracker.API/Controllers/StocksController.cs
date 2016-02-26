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
     * dev:
     * 4sUbT0euzBm-cc-fBOn3Kjt7TsTVm-QjOFVCKYV1O37JS8FPb8NJb-2WNz2pC6k7IinTvfLQL39qioEuIjJ7Qh4X0W4fenbY6NOTXasuX3rXtOnxfHdzD63UQIE7QZN82kzSMcxfJDIp8HUE7KkL9pgutCV5vvQcVUoUq-RciMsB6uCQohozHMZfZt7JOQJIVSLXXUVlA0dNQ5Mk644X8sBRXO5aSWT1RLJA8Rww8AgUdSn3mpu4N16G5cgAYSqBiIs1sKdLqL3sg2DMfE5ezyYVtv-3WZgdNWPeHYwq6ikQJmrASWbZjttS4qtXG5awVkJZ0Xo1f3DtOT_Urx-UXAF9FUNEp1Q63eIqbV2FehQSyXLTyJtvX3Ixd2ncnlR0d5Z_hcOjfYMBO5OFmOUxYhyjQe5jLTDY7fF3wRp3arkQ02CObBxtjBShcMXfvnh4jGbrmS_Rj0b1KkPzj2-p1PvsKlNophjXpENs75sZNRJHstw5kwsAAdoTh8iak_OBloanDV5hX6Yt9e6LYpQBsg
     * 
     * prod:
     * kBFZ9V8P79QhEa5EH-5qQJXVgASmR0b-dl8QJ_lcSQJSIhX7GkI2QBvhfFKs-r_SdPF4V8Ts5u0oB2fXtgH9AqrIFlCR4pcYXVZFVGk1NDwoSKp40vsGtcZThwgOCiC6LzlQ0BkjhXp9jrpUFEKvUmp0kl3U_27dER2TUnRRGYPKoWhmyw7EYg3o_DK9tSrzJhs38M2cfyEa83AcC5jqyU6vGS9IRy1-9l1qW_DEi13BMnKyhyaj7EZu7_xvMi90JOGAXhQcSyAJSPUC_SgOhG-1c0C77Uf1OdGsZ7Kd1Qw9svxxFk4IKr44ynmdItvHhXU8MfRP_ffOKvP1kByVFZnWFC6IC4obWPLp_48Hjhvo6YJ1LeM7d5pKKR0bz6z3QozejB_ELCYFUIUvH--49MkXoe4I12vFJcDavpASmtsAMNWnAEJWJKhyE74v2cDvwcpwJpeaNkZbhrCZFTKh2Q5HYhfQuZR-9O_Yzne1FRNTNoHR7bv9gOO0agpudUb7ftkqT9I9fUFRJWzQOg8kAA
     */
