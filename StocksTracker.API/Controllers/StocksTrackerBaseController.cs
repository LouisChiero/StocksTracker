using System;
using System.Linq;
using System.Web.Http;
using Core.Framework.API.Stocks;

namespace StocksTracker.API.Controllers
{
    /// <summary>
    /// Custom base class extends <see cref="ApiController"/> with methods common to all Stocks Tracker controllers.
    /// </summary>
    public class StocksTrackerBaseController : ApiController
    {
        protected const string GetStockRouteName = "GetStockById";
        protected const string GetStockTrackerRouteName = "GetStockTrackerById";

        protected string BuildUrl(string routeName, object routeValues)
        {   // new { id = 2}
            // new { id = measure.Food.Id, id = measure.Id }
            return Url.Link(routeName, routeValues);
        }

        protected object MapStockRecordToObject(StockRecord stock)
        {
            return new
            {
                id = stock.StockRecordId,
                name = stock.Name,
                ticker = stock.TickerSymbol,
                openPrice = stock.OpenPrice,
                lastTrade = stock.LastTrade,
                dayHigh = stock.DayHigh,
                dayLow = stock.DayLow,
                yearHigh = stock.YearHigh,
                yearLow = stock.YearLow,
                previousClose = stock.PreviousClose,
                lastUpdated = stock.LastUpdatedDateTimeUniversal.HasValue ? stock.LastUpdatedDateTimeUniversal.Value.ToLocalTime().Date : new DateTime?(),
                url = BuildUrl(GetStockRouteName, new{id = stock.StockRecordId})
            };
        }

        protected object MapStockTrackerRecordToObject(StockTrackerRecord stockTracker)
        {
            return new
            {
                id = stockTracker.StockTrackerRecordId,
                name = stockTracker.Name,
                isDefault = stockTracker.IsDefault,
                stocks = stockTracker.Stocks != null ? stockTracker.Stocks.Select(MapStockRecordToObject).ToArray() : new object[]{},
                url = BuildUrl(GetStockTrackerRouteName, new {id = stockTracker.StockTrackerRecordId})
            };
        }
    }
}