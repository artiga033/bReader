using bReader.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared
{
    public static class Extensions
    {
        public static List<FeedDto> GetAllFeeds(this IEnumerable<FeedGroupDto> groups)
        {
            List<FeedDto> r = new();
            foreach (var g in groups)
            {
                foreach (var f in g.Feeds)
                {
                    r.Add(f);
                }
            }
            return r;
        }
        public static List<Feed> GetAllFeeds(this IEnumerable<FeedGroup> groups)
        {
            List<Feed> r = new();
            foreach (var g in groups)
            {
                foreach (var f in g.Feeds)
                {
                    r.Add(f);
                }
            }
            return r;
        }
        public static FeedItemDto ToFeedItemDto(this SyndicationItem synFeedItem)
        {
            FeedItemDto result = new FeedItemDto();
            throw new NotImplementedException();
        }
        public static FeedDto ToFeedDto(this SyndicationFeed synFeed)
        {
            throw new NotImplementedException();
        }
    }
}
