using System.Xml.Linq;


namespace bReader.Shared.Models.Parser
{
    public static class Rss20FeedParser
    {
        public static readonly ISet<string> RecognizedVersions = new HashSet<string>
        {
            "2.0",

            // support previous RSS 2.* branch versions
            "0.91",
            "0.92",
            "0.93",
            "0.94",
        };
        public static bool TryParseRss20Feed(XDocument document, out FeedDto parsedFeed)
        {
            parsedFeed = new FeedDto();

            var rssElement = document?.Element("rss");
            if (rssElement == null)
                return false;

            var rssVersion = rssElement.Attribute("version")?.Value;

            if (rssVersion == null || !RecognizedVersions.Contains(rssVersion))
                return false;

            return TryParseRss20Channel(rssElement.Element("channel"), ref parsedFeed);
        }

        private static bool TryParseRss20Channel(XElement? channelElement, ref FeedDto parsedFeed)
        {

            if (channelElement == null)
                return false;

            parsedFeed.Title = channelElement.Element("title")?.Value.Trim() ?? parsedFeed.Title;
            var link = channelElement.Element("link");
            parsedFeed.Description = channelElement.Element("description")?.Value.Trim() ?? parsedFeed.Description;
            parsedFeed.Language = channelElement.Element("language")?.Value.Trim() ?? parsedFeed.Language;
            parsedFeed.Copyright = channelElement.Element("copyright")?.Value.Trim() ?? parsedFeed.Copyright;
            parsedFeed.Generator = channelElement.Element("generator")?.Value.Trim() ?? parsedFeed.Generator;
            var e = channelElement.Element("managingEditor");
            var m = channelElement.Element("webMaster");
            var d = channelElement.Element("docs");
            var i = channelElement.Element("image")?.Element("url");
            if (link != null)
                parsedFeed.Links.Add(new Uri(link.Value.Trim()));
            if (e != null)
                parsedFeed.Authors.Add(new PersonDto() { Name = e.Value.Trim() });
            if (m != null)
                parsedFeed.Authors.Add(new PersonDto() { Name = m.Value.Trim() });
            if (d != null)
                parsedFeed.Documentation = new Uri(d.Value.Trim());
            if (i != null)
                parsedFeed.ImageUrl = new Uri(i.Value.Trim());

            if (TryParseRss20Timestamp(channelElement.Element("lastBuildDate"), out var parsedLastBuildDate))
            {
                parsedFeed.LastUpdatedTime = parsedLastBuildDate;
            }

            foreach (var categoryElement in channelElement.Elements("category"))
            {
                if (TryParseRss20Category(categoryElement, out var parsedCategory))
                {
                    parsedFeed.Categories.Add(parsedCategory);
                }
            }
            // items
            foreach (var itemElement in channelElement.Elements("item"))
            {
                if (TryParseRss20Item(itemElement, out var parsedItem))
                {
                    parsedFeed.Items.Add(parsedItem);
                }
            }

            return true;
        }

        private static bool TryParseRss20Category(XElement categoryElement, out CategoryDto parsedCategory)
        {
            parsedCategory = new CategoryDto();

            if (categoryElement == null)
                return false;

            parsedCategory.Name = categoryElement.Value.Trim();
            parsedCategory.Scheme = categoryElement.Attribute("domain")?.Value;

            return true;
        }

        private static bool TryParseRss20Timestamp(XElement? timestampElement, out DateTimeOffset parsedTimestamp)
        {
            parsedTimestamp = default;

            if (timestampElement == null)
                return false;

            if (!RelaxedTimestampParser.TryParseTimestampFromString(timestampElement.Value, out parsedTimestamp))
                return false;

            return true;
        }

        private static bool TryParseRss20Item(XElement itemElement, out FeedItemDto parsedItem)
        {
            parsedItem = new FeedItemDto();
            if (itemElement == null)
                return false;

            parsedItem.Title = itemElement.Element("title")?.Value.Trim() ?? parsedItem.Title;
            parsedItem.Summary = itemElement.Element("description")?.Value.Trim() ?? parsedItem.Summary;
            var l = itemElement.Element("link");
            var a = itemElement.Element("author");
            var i = itemElement.Element("guid");
            if (l != null)
                parsedItem.Links.Add(new Uri(l.Value.Trim()));
            if (a != null)
                parsedItem.Authors.Add(new PersonDto() { Name = a.Value.Trim() });
            if (i != null)
                parsedItem.Id = i.Value.Trim();
            foreach (var categoryElement in itemElement.Elements("category"))
            {
                if (TryParseRss20Category(categoryElement, out var parsedCategory))
                {
                    parsedItem.Categories.Add(parsedCategory);
                }
            }

            if (TryParseRss20Timestamp(itemElement.Element("pubDate"), out var parsedPubDate))
            {
                parsedItem.PublishDate = parsedPubDate;
            }

            return true;
        }

        private static bool TryParseRss20BoolValue(string boolValue, out bool parsedValue)
        {
            parsedValue = false;

            if (string.IsNullOrWhiteSpace(boolValue))
                return false;

            switch (boolValue.Trim().ToLowerInvariant())
            {
                case "true":
                    parsedValue = true;
                    return true;
                case "false":
                    return true;
                default:
                    return false;
            }
        }
    }
}
