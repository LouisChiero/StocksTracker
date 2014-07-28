using System.ComponentModel.DataAnnotations;

namespace StocksTracker.API.Models
{
    /// <summary>
    /// Class defines a stock tracker model for the API.
    /// </summary>
    public class StockTrackerModel
    {
        /// <summary>
        /// Gets and set the stock tracker name value.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Stock Tracker name must be greater than zero characters, and less than or equal to 100 characters.")]
        public string StockTrackerName { get; set; }

        /// <summary>
        /// Gets and sets the default value.
        /// </summary>
        public bool Default { get; set; }
    }
}