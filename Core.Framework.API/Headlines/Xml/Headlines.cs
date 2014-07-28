using System;
using System.Xml.Serialization;

namespace Core.Framework.API.Headlines.Xml
{
    [Serializable, XmlRoot("rss")]
    public class Headlines
    {
        [XmlElement("channel")]
        public Channel Channel { get; set; }
    }

    [Serializable]
    public class Channel
    {
        [XmlElement("item")]
        public Item[] Items { get; set; }
    }

    [Serializable]
    public class Item
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("pubDate")]
        public string PubDate { get; set; }
    }
}
