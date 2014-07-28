using System.Collections.Generic;

namespace Data.Models
{
    /// <summary>
    /// Encapsulates properties of an object that can track <see cref="Stock"/> objects.
    /// </summary>
    public class StockTracker
    {
        /// <summary>
        /// Instantiates the StockTracker class.
        /// </summary>
        public StockTracker()
        {
            StockTrackerStocks = new HashSet<StockTrackerStock>();
        }

        /// <summary>
        /// Gets and sets the stock tracker ID value.
        /// </summary>
        public int StockTrackerId { get; set; }

        /// <summary>
        /// Gets and sets the application user ID value.
        /// </summary>
        public string ApplicationUserId { get; set; }

        /// <summary>
        /// Gets and sets the stock tracker name value.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets is default value.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets and sets the <see cref="ApplicationUser"/> value.
        /// </summary>
        public ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// Gets and sets a collection of <see cref="StockTrackerStock"/> objects.
        /// </summary>
        public ICollection<StockTrackerStock> StockTrackerStocks { get; set; }
    }
}
