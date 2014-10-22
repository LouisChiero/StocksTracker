using System;
using System.Data.Entity.Core;
using System.Globalization;
using System.Linq;
using Core.Framework.API.Charts;
using Core.Framework.API.Data;
using Core.Framework.API.Stocks;

namespace StocksTracker.API.Services
{
    /// <summary>
    /// Implementation of <see cref="IStockChartsService"/> interface, which queries stock charts via a public API.
    /// </summary>
    public class StockChartsService : IStockChartsService
    {
        private readonly ICache<StockRecord> _stockCache;

        public StockChartsService(ICache<StockRecord> stockCache)
        {
            _stockCache = stockCache;
        }

        /// <see cref="IStockChartsService.GetStockChartResource"/>
        public Uri GetStockChartResource(StockChartQueryParameters chartQueryParameters)
        {
            StockRecord stock;
            if (chartQueryParameters.QueryById)
            {
                stock = _stockCache.GetById(chartQueryParameters.StockId);
            }
            else
            {
                stock =
                    _stockCache.GetAll()
                        .SingleOrDefault(
                            record =>
                                String.Equals(record.TickerSymbol, chartQueryParameters.TickerSymbol,
                                    StringComparison.InvariantCultureIgnoreCase));
            }

            if (stock == null)
                throw new ObjectNotFoundException();

            return new Uri(CreateUrl(stock.TickerSymbol, chartQueryParameters));
        }

        private static string CreateUrl(string tickerSymbol, StockChartQueryParameters stockChartQueryParameters)
        {
            #region converter funcs
            Func<StockChartTimeSpan, string> timeSpanConverter = span =>
                    {
                        switch (span)
                        {
                            case StockChartTimeSpan.OneDay:
                                return "1d";
                            case StockChartTimeSpan.FiveDays:
                                return "5d";
                            case StockChartTimeSpan.OneMonth:
                                return "1m";
                            case StockChartTimeSpan.ThreeMonths:
                                return "3m";
                            case StockChartTimeSpan.SixMonths:
                                return "6m";
                            case StockChartTimeSpan.OneYear:
                                return "1y";
                            case StockChartTimeSpan.TwoYears:
                                return "2y";
                            case StockChartTimeSpan.FiveYears:
                                return "5y";
                            case StockChartTimeSpan.Maximum:
                                return "my";
                        }

                        return null;
                    };

            Func<StockChartType, string> chartTypeConverter = type =>
                {
                    switch (type)
                    {
                        case StockChartType.Bar:
                            return "b";
                        case StockChartType.Candle:
                            return "c";
                        case StockChartType.Line:
                            return "l";
                    }

                    return null;
                };

            Func<StockChartSize, string> chartSizeConverter = size =>
                {
                    switch (size)
                    {
                        case StockChartSize.Large:
                            return "l";
                        case StockChartSize.Middle:
                            return "m";
                        case StockChartSize.Small:
                            return "s";
                    }

                    return null;
                }; 
            #endregion

            return string.Format(CultureInfo.InvariantCulture,
                "http://chart.finance.yahoo.com/z?s={0}&t={1}&q={2}&z={3}", tickerSymbol,
                timeSpanConverter(stockChartQueryParameters.ChartTimeSpan),
                chartTypeConverter(stockChartQueryParameters.ChartType),
                chartSizeConverter(stockChartQueryParameters.ChartSize));
        }
    }
}