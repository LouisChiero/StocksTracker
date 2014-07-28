namespace StocksTracker.API.Models
{
    /// <summary>
    /// Class defines an API model for updating a stock tracker.
    /// </summary>
    public class UpdateStockTrackerModel : StockTrackerModel
    {
        /// <summary>
        /// Gets and sets the collection of <see cref="StockModel"/> objects.
        /// </summary>
        public StockModel[] Stocks { get; set; }
    }
}