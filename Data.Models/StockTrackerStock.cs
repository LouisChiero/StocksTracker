namespace Data.Models
{
    /// <summary>
    /// Encapsulates the many-to-many relationship between a <see cref="StockTracker"/> and <see cref="Stock"/> object.
    /// </summary>
    public class StockTrackerStock
    {
        /// <summary>
        /// Gets and sets the stock tracker ID value.
        /// </summary>
        public int StockTrackerId { get; set; }

        /// <summary>
        /// Gets and sets the stock ID value.
        /// </summary>
        public int StockId { get; set; }

        /// <summary>
        /// Gets and sets the <see cref="StockTracker"/> value.
        /// </summary>
        public virtual StockTracker StockTracker { get; set; }

        /// <summary>
        /// Gets and sets the <see cref="Stock"/> value.
        /// </summary>
        public virtual Stock Stock { get; set; }
    }
}
