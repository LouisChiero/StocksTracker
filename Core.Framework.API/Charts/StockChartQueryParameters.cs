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
        /// <param name="stockId">The stock ID.</param>
        /// <param name="chartTimeSpan">The chart time span.</param>
        /// <param name="chartType">The chart type.</param>
        /// <param name="chartSize">The chart size.</param>
        public StockChartQueryParameters(
            int stockId,
            StockChartTimeSpan chartTimeSpan,
            StockChartType chartType,
            StockChartSize chartSize)
        {
            StockId = stockId;
            ChartTimeSpan = chartTimeSpan;
            ChartType = chartType;
            ChartSize = chartSize;
        }

        /// <summary>
        /// Gets the ticker symbol value.
        /// </summary>
        public int StockId { get; private set; }

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
    }
}
