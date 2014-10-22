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
     * _nP-_9ybjhVMXOHKjX-Uz9SdPQprH7dJoGIOelkZ1umiAbgXIfQ-HNXiuxCFGINs9ax0YQ7eV2RxcOUvtXpkv08OfMvEWK9LtYOLkmYfA0eh8SpiHenK0n1lrxedd019k1fdBgrzM6kp0AwHh__73nl1pIct_BJV6BH1h0UbNmkK8A_OVR-HPiYJiO1ZhAIVXHDJz8pUHDvE8aP0sM-IwKZM0O48UU1NXtTN9kJ2Co1i5e6JmuYA2EygD8nO15y86ViKAu7-s2mteIBgrNjZCjlowMsemKYgWktg48baIPybkqRKRCMxFfFBr9PeOWJyaUj_LlHEexn8050SNJnfLpwJmTPhyEaKCdrCOARumZQtc2P6cChsuXAO7gzXmuuRvjAEHfNiLcERXvR3xvJnyF8Z1ETFZpcbsGfv8-DimMKjWaHXHhOl9ys8oWuvL_ocncCjq5yC-fO5txCOVIglkeqXxDLfekrEd5md72CEq-w
     */
