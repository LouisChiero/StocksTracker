using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Framework.API.Stocks;
using Microsoft.AspNet.Identity;
using StocksTracker.API.Models;

namespace StocksTracker.API.Controllers
{
    /// <summary>
    /// Controller provides methods for manipulating stock trackers using HTTP protocols.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/StockTrackers")]
    public class StockTrackersController : StocksTrackerBaseController
    {
        private readonly IStockTrackers _stockTrackers;

        /// <summary>
        /// Instantiates the StockTrackersController class.
        /// </summary>
        /// <param name="stockTrackers">Reference to an object that provides CRUD actions for stock trackers.</param>
        public StockTrackersController(IStockTrackers stockTrackers)
        {
            _stockTrackers = stockTrackers;
        }

        // GET api/StocksTrackers
        [HttpGet]
        [Route]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var stockTrackers = await _stockTrackers.GetStockTrackersForUser(User.Identity.GetUserId());
                return Ok(stockTrackers.Select(MapStockTrackerRecordToObject));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/StockTrackers/1
        [HttpGet]
        [Route("{id:int}", Name = GetStockTrackerRouteName)]
        public async Task<IHttpActionResult> Get(int id)
        {
            if (id == 0)
                return BadRequest();

            StockTrackerRecord stockTracker;
            try
            {
                stockTracker = await _stockTrackers.GetStockTrackerAsync(id);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            if (stockTracker == null)
                return NotFound();

            return Ok(MapStockTrackerRecordToObject(stockTracker));
        }

        // POST api/StockTrackers/Create
        [Route("Create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(StockTrackerModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newStockTracker =
                    await
                        _stockTrackers.AddStockTrackerForUserAsync(
                            new StockTrackerRecord {IsDefault = model.Default, Name = model.StockTrackerName},
                            User.Identity.GetUserId());

                var url = BuildUrl(GetStockTrackerRouteName, new {id = newStockTracker.StockTrackerRecordId});
                return Created(url, MapStockTrackerRecordToObject(newStockTracker));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE api/StockTrackers/1
        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest();

            try
            {
                var removeResult = await _stockTrackers.RemoveStockTrackerAsync(id);
                if (removeResult != StockTrackerOperationResult.RemoveSucceeded)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", "Removing Stock Tracker failed.");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok();
        }

        // PUT api/StockTrackers/1
        [HttpPatch]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, UpdateStockTrackerModel model)
        {
            if (id == 0)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            StockTrackerRecord stockTracker;
            try
            {
                var stocks = model.Stocks.Select(stockModel => new StockRecord {StockRecordId = stockModel.StockId});
                stockTracker =
                    await
                        _stockTrackers.UpdateStockTrackerAsync(new StockTrackerRecord
                        {
                            IsDefault = model.Default,
                            Name = model.StockTrackerName,
                            StockTrackerRecordId = id,
                            Stocks = stocks.ToArray()
                        });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            if (stockTracker == null)
                return NotFound();

            return Ok(MapStockTrackerRecordToObject(stockTracker));
        }
    }
}
