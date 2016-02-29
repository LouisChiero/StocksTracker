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
     * SCoZel125jGclnYCBmzsCSgwyRy3B-E53ncnNkLsS3egIYpOkRNVxvAh08FS3hvdCc67ST3yi31w5KoMRfbnOIuOWAR-CEf9Kc8WvH-DS1uk4pUqiP8L0FY6Uq20kl8YwWbk0MSj_e4RPlJ5KZmSdYto5z_xevdJU85C_YEBabz5A2vYZGlpFqdUA5QflOYlYTNTs9rStqZFdec2pDPSO5pyNc5xk8GKRDrdJyrJjzZBC0GmrztMTjUScQsCWm_6Hpr7u6-R7KtjWs_fiQLlbp3GXZJpxP6RLSD49UfKpLnY7UJLhdpIod0g8rmomzsKExD9ZzoysN_X_Ai_yqtOW5ehA3nNJirz6uaDq1pkwxUfKJqMJzGwvEGfS7JwoUht1PPYV__Wnmzqh9plOIEN0kq1-mqD4DK96VjAMI507VPOFvuYE7AW0joPS-4MHvaMylvNXYntBvkYq4KHijfyKf-U9cYvC7CKa_5ULphhHdiDO6GeIeNs10SBdBJ8CzmuokEbNK7ixRv-KwN6FRDIXQ
     * 
     * prod:
     * kBFZ9V8P79QhEa5EH-5qQJXVgASmR0b-dl8QJ_lcSQJSIhX7GkI2QBvhfFKs-r_SdPF4V8Ts5u0oB2fXtgH9AqrIFlCR4pcYXVZFVGk1NDwoSKp40vsGtcZThwgOCiC6LzlQ0BkjhXp9jrpUFEKvUmp0kl3U_27dER2TUnRRGYPKoWhmyw7EYg3o_DK9tSrzJhs38M2cfyEa83AcC5jqyU6vGS9IRy1-9l1qW_DEi13BMnKyhyaj7EZu7_xvMi90JOGAXhQcSyAJSPUC_SgOhG-1c0C77Uf1OdGsZ7Kd1Qw9svxxFk4IKr44ynmdItvHhXU8MfRP_ffOKvP1kByVFZnWFC6IC4obWPLp_48Hjhvo6YJ1LeM7d5pKKR0bz6z3QozejB_ELCYFUIUvH--49MkXoe4I12vFJcDavpASmtsAMNWnAEJWJKhyE74v2cDvwcpwJpeaNkZbhrCZFTKh2Q5HYhfQuZR-9O_Yzne1FRNTNoHR7bv9gOO0agpudUb7ftkqT9I9fUFRJWzQOg8kAA
     */
