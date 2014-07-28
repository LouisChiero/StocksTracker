using System;

namespace Core.Framework.API.Charts
{
    /// <summary>
    /// Interface defines actions for stock charts.
    /// </summary>
    public interface IStockChartsService
    {
        /// <summary>
        /// Returns a <see cref="Uri"/> reference to a chart resource for the given query parameters.
        /// </summary>
        /// <param name="chartQueryParameters">The stock chart query parameters.</param>
        /// <returns>A <see cref="Uri"/> object.</returns>
        Uri GetStockChartResource(StockChartQueryParameters chartQueryParameters);
    }
}
