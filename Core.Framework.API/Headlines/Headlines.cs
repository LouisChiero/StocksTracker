namespace Core.Framework.API.Headlines
{
    /// <summary>
    /// Class encapsulates headline data.
    /// </summary>
    public class Headlines
    {
        /// <summary>
        /// Instantiates the Headlines class.
        /// </summary>
        /// <param name="tickerSymbol">The ticker symbol for the headlines.</param>
        /// <param name="stockName">The stock name for the headlines.</param>
        /// <param name="newsItems"><see cref="NewsItem"/>s for the headlines.</param>
        public Headlines(
            string tickerSymbol,
            string stockName, 
            NewsItem[] newsItems)
        {
            TickerSymbol = tickerSymbol;
            StockName = stockName;
            NewsItems = newsItems;
        }

        /// <summary>
        /// Gets the ticker symbol value.
        /// </summary>
        public string TickerSymbol { get; private set; }

        /// <summary>
        /// Gets the stock name value,
        /// </summary>
        public string StockName { get; private set; }

        /// <summary>
        /// Gets the news items value.
        /// </summary>
        public NewsItem[] NewsItems { get; set; }
    }
}
