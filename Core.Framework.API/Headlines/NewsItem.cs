namespace Core.Framework.API.Headlines
{
    /// <summary>
    /// Class encapsulates news item data.
    /// </summary>
    public class NewsItem
    {
        /// <summary>
        /// Instantiates the NewsItem class.
        /// </summary>
        /// <param name="title">The title of the news item.</param>
        /// <param name="link">The link of the news item.</param>
        /// <param name="description">The description of the news item.</param>
        /// <param name="pubDate">The publish date of the news item.</param>
        public NewsItem(string title, string link, string description, string pubDate)
        {
            Title = title;
            Link = link;
            Description = description;
            PubDate = pubDate;
        }

        /// <summary>
        /// Gets the title value.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the link value.
        /// </summary>
        public string Link { get; private set; }

        /// <summary>
        /// Gets the description value.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the pub date value.
        /// </summary>
        public string PubDate { get; private set; }
    }
}
