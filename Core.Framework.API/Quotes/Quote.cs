using System;
using System.Globalization;

namespace Core.Framework.API.Quotes
{
    /// <summary>
    /// Class encapsulates stock quote data.
    /// </summary>
    public class Quote
    {
        private readonly string[] _quoteFields;

        /// <summary>
        /// Instantiates the Quote class.
        /// </summary>
        /// <param name="delimitedQuoteString">A delimited string of quote data.</param>
        /// <param name="delimiter">The delimiter used to parse quote data.</param>
        public Quote(string delimitedQuoteString, string delimiter = ",")
        {
            _quoteFields = delimitedQuoteString.Split(new[] { delimiter }, StringSplitOptions.None);
        }

        /// <summary>
        /// Gets the quote ticker value.
        /// </summary>
        public string Ticker { get { return _quoteFields[0].Replace("\"", string.Empty); } }

        /// <summary>
        /// Gets the quote open value.
        /// </summary>
        public decimal? Open
        {
            get
            {
                decimal d;
                if (decimal.TryParse(_quoteFields[1].Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out d))
                    return d;

                return null;
            }
        }

        /// <summary>
        /// Gets the quote last trade value.
        /// </summary>
        public decimal? LastTrade
        {
            get
            {
                decimal d;
                if (decimal.TryParse(_quoteFields[2].Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out d))
                    return d;

                return null;
            }
        }

        /// <summary>
        /// Gets the quote day high value.
        /// </summary>
        public decimal? DayHigh
        {
            get
            {
                decimal d;
                if (decimal.TryParse(_quoteFields[3].Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out d))
                    return d;

                return null;
            }
        }

        /// <summary>
        /// Gets the quote day low value.
        /// </summary>
        public decimal? DayLow
        {
            get
            {
                decimal d;
                if (decimal.TryParse(_quoteFields[4].Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out d))
                    return d;

                return null;
            }
        }

        /// <summary>
        /// Gets the quote year high value.
        /// </summary>
        public decimal? YearHigh
        {
            get
            {
                decimal d;
                if (decimal.TryParse(_quoteFields[5].Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out d))
                    return d;

                return null;
            }
        }

        /// <summary>
        /// Gets the quote year low value.
        /// </summary>
        public decimal? YearLow
        {
            get
            {
                decimal d;
                if (decimal.TryParse(_quoteFields[6].Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out d))
                    return d;

                return null;
            }
        }

        /// <summary>
        /// Gets the quote previous close value.
        /// </summary>
        public decimal? PreviousClose
        {
            get
            {
                decimal d;
                if (decimal.TryParse(_quoteFields[7].Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out d))
                    return d;

                return null;
            }
        }
    }
}
