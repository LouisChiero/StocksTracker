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
                return Ok(stocks.Select(MapStockRecordToObject));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/Stocks/1
        [HttpGet]
        [Route("{id:int}", Name = GetStockRouteName)]
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
     * VujGot3QEUY6tB4GPoevwHlHVOJ7Ezed0gs_3x-N_Cp7DX1zwnAssLO4vw4E5ulIhckh8GFQawZHHMWKoN8arC0HzENbmxKMwQkPHUEENPE3wxSrfkRUhnLvo3GwLoEjJZXx9jbGhKVTVmoEOVJhihpXz9Zz75RFpcUoV-ceRSOJF1W_MW8zKAxOAuvTFWSIdxvnUeG7uUFiP7jAUk87ZrZ1AUj5wG6Uwk-6P_yMfGgA_h75q67O360kJQ86S0IDAPNd7DqaA3wRvNScinxIIp-j76tqiEFc5SjTQlXoaezfiQTISy2z8BTwg3cnG2Ir-EzhvzLSlaGdkasdkYYISbWxvxaXUZd2bTJIW8RFe3WnrwdsDeWbjEqjAG2UE-cH0xi3Wgn-TqUpr_kpK3Zzq0vKRJE3UZBaWGcSj20nY3j40sdUfuQTkn7VHg9M7Re8J95F-4ws_d-1fncLPF44HdUeh_675MjVyIDhXYXqZKshkYY_423xVIkUqPyR2sNXmO8AxTAvZlRhkhZ0laYrRw
     */
