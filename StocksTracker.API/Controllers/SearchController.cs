using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Core.Framework.API.Stocks;

namespace StocksTracker.API.Controllers
{
    /// <summary>
    /// Controller provides search methods for stocks using HTTP protocols.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Search")]
    public class SearchController : StocksTrackerBaseController
    {
        private readonly IStocks _stocksService;

        /// <summary>
        /// Instantiates the SearchController class.
        /// </summary>
        /// <param name="stocksService">Reference to an object that queries stocks.</param>
        public SearchController(IStocks stocksService)
        {
            _stocksService = stocksService;
        }
        
        // GET api/Search/MSFT
        // GET api/Search/Microsoft
        [Route("{term}")]
        [HttpGet]
        public async Task<JsonResult<IEnumerable<dynamic>>> Get(string term)
        {
            var results = await _stocksService.GetStocksSearchValuesAsync(HttpUtility.HtmlEncode(term));
            return Json(results);
        }
    }
}
