using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bReader.Shared.Models.Parser
{
    public static class FeedParser
    {
        public static FeedDto ParseToFeedDto(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            FeedDto result;
            if (!Rss20FeedParser.TryParseRss20Feed(doc, out result))
                if (!Atom10FeedParser.TryParseAtom10Feed(doc, out result))
                    throw new ArgumentException("Unrecognizable feed format");
            return result;
        }
    }
}
