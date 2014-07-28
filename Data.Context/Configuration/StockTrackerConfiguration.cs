using System.Data.Entity.ModelConfiguration;
using Data.Models;

namespace Data.Context.Configuration
{
    /// <summary>
    /// Defines the entity type configuration for the <see cref="StockTracker"/> domain model.
    /// </summary>
    public class StockTrackerConfiguration : EntityTypeConfiguration<StockTracker>
    {
        /// <summary>
        /// Instantiates the StockTrackerConfiguration class.
        /// </summary>
        public StockTrackerConfiguration()
        {
            HasKey(st => st.StockTrackerId);

            Property(st => st.ApplicationUserId)
                .IsRequired();

            Property(st => st.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(st => st.IsDefault);

            HasMany(st => st.StockTrackerStocks)
                .WithRequired(sts => sts.StockTracker)
                .HasForeignKey(fk => fk.StockTrackerId);

            ToTable("StockTracker");
        }
    }
}
