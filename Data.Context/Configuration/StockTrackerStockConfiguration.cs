using System.Data.Entity.ModelConfiguration;
using Data.Models;

namespace Data.Context.Configuration
{
    /// <summary>
    /// Defines the entity type configuration for the <see cref="StockTrackerStock"/> domain model.
    /// </summary>
    public class StockTrackerStockConfiguration : EntityTypeConfiguration<StockTrackerStock>
    {
        /// <summary>
        /// Instantiates the StockTrackerStockConfiguration class.
        /// </summary>
        public StockTrackerStockConfiguration()
        {
            HasKey(sts => new { sts.StockId, sts.StockTrackerId });

            ToTable("StockTrackerStock");
        }
    }
}
