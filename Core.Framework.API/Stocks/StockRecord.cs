using System;

namespace Core.Framework.API.Stocks
{
    /// <summary>
    /// Class provides access to properties of a stock record.
    /// </summary>
    public class StockRecord
    {
        /// <summary>
        /// Gets and sets the stock record id value.
        /// </summary>
        public int StockRecordId { get; set; }

        /// <summary>
        /// Gets and sets the ticker symbol value.
        /// </summary>
        public string TickerSymbol { get; set; }

        /// <summary>
        /// Gets and sets the stock name value.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the stock open price value.
        /// </summary>
        public decimal? OpenPrice { get; set; }

        /// <summary>
        /// Gets and sets the stock last trade value.
        /// </summary>
        public decimal? LastTrade { get; set; }

        /// <summary>
        /// Gets and sets the stock day high value.
        /// </summary>
        public decimal? DayHigh { get; set; }

        /// <summary>
        /// Gets and sets the stock day low value.
        /// </summary>
        public decimal? DayLow { get; set; }

        /// <summary>
        /// Gets and sets the stock year high value.
        /// </summary>
        public decimal? YearHigh { get; set; }

        /// <summary>
        /// Gets and sets the stock year low value.
        /// </summary>
        public decimal? YearLow { get; set; }

        /// <summary>
        /// Gets and sets the stock previous close value.
        /// </summary>
        public decimal? PreviousClose { get; set; }

        /// <summary>
        /// Gets and sets the stock last updated value.
        /// </summary>
        public DateTimeOffset? LastUpdatedDateTimeUniversal { get; set; }
    }
}
