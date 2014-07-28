namespace Core.Framework.API.Stocks
{
    /// <summary>
    /// Class provides access to properties of a stock tracker record.
    /// </summary>
    public class StockTrackerRecord
    {
        /// <summary>
        /// Gets and sets the stock tracker record ID value.
        /// </summary>
        public int StockTrackerRecordId { get; set; }

        /// <summary>
        /// Gets and sets the stock tracker name value.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets is default value.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets and sets a collection of <see cref="StockRecord"/> objects.
        /// </summary>
        public StockRecord[] Stocks { get; set; }
    }
}
