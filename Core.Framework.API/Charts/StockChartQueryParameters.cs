namespace Core.Framework.API.Charts
{
    /// <summary>
    /// Class defines parameters used to query stock charts.
    /// </summary>
    public class StockChartQueryParameters
    {
        /// <summary>
        /// Instantiates the StockChartQueryParameters class.
        /// </summary>
        /// <param name="chartTimeSpan">The chart time span.</param>
        /// <param name="chartType">The chart type.</param>
        /// <param name="chartSize">The chart size.</param>
        /// <param name="stockId">The stock ID.</param>
        /// <param name="tickerSymbol">The stock ticker symbol.</param>
        public StockChartQueryParameters(
            StockChartTimeSpan chartTimeSpan,
            StockChartType chartType,
            StockChartSize chartSize,
            int stockId = 0,
            string tickerSymbol = null)
        {
            StockId = stockId;
            TickerSymbol = tickerSymbol;
            ChartTimeSpan = chartTimeSpan;
            ChartType = chartType;
            ChartSize = chartSize;
            QueryById = stockId > 0 && string.IsNullOrWhiteSpace(tickerSymbol);
        }

        /// <summary>
        /// Gets the stock ID value.
        /// </summary>
        public int StockId { get; private set; }

        /// <summary>
        /// Gets the ticker symbol value.
        /// </summary>
        public string TickerSymbol { get; private set; }

        /// <summary>
        /// Gets the chart time span value.
        /// </summary>
        public StockChartTimeSpan ChartTimeSpan { get; private set; }

        /// <summary>
        /// Gets the chart type value.
        /// </summary>
        public StockChartType ChartType { get; private set; }

        /// <summary>
        /// Gets the chart size value.
        /// </summary>
        public StockChartSize ChartSize { get; private set; }

        /// <summary>
        /// Gets the query by ID value.
        /// </summary>
        public bool QueryById { get; private set; }
    }
}
