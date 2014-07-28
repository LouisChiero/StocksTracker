using System.Data.Entity.ModelConfiguration;
using Data.Models;

namespace Data.Context.Configuration
{
    /// <summary>
    /// Defines the entity type configuration for the <see cref="Stock"/> domain model.
    /// </summary>
    public class StockConfiguration : EntityTypeConfiguration<Stock>
    {
        /// <summary>
        /// Instantiates the StockConfiguration class.
        /// </summary>
        public StockConfiguration()
        {
            HasKey(s => s.StockId);

            Property(s => s.TickerSymbol)
                .IsRequired()
                .HasMaxLength(10);

            Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(s => s.OpenPrice)
                .IsOptional()
                .HasPrecision(8, 2);

            Property(s => s.LastTrade)
                .IsOptional()
                .HasPrecision(8, 2);

            Property(s => s.DayHigh)
                .IsOptional()
                .HasPrecision(8, 2);

            Property(s => s.DayLow)
                .IsOptional()
                .HasPrecision(8, 2);

            Property(s => s.YearHigh)
                .IsOptional()
                .HasPrecision(8, 2);

            Property(s => s.YearLow)
                .IsOptional()
                .HasPrecision(8, 2);

            Property(s => s.PreviousClose)
                .IsOptional()
                .HasPrecision(8, 2);

            Property(s => s.LastUpdatedDateTimeUniversal)
                .IsOptional()
                .HasColumnType("datetimeoffset");

            HasMany(s => s.StockTrackerStocks)
                .WithRequired(sts => sts.Stock)
                .HasForeignKey(fk => fk.StockId);

            ToTable("Stock");
        }
    }
}
